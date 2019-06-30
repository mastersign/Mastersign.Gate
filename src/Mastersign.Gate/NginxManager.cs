using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mastersign.Gate
{
    public class NginxManager
    {
        private const string NGINX_ORG_DOWNLOAD_URL = "https://nginx.org/en/download.html";
        private const string NGINX_ORG_DOWNLOAD_XPATH = "//div[@id='main']/descendant::div[@id='content']/descendant::table/descendant::td/a";
        private static readonly Regex NGINX_ORG_DOWNLOAD_RE = new Regex(@"nginx.*Windows.*?(?<version>\d+(?:\.\d+){1,3})", RegexOptions.IgnoreCase);
        private static readonly Regex NGINX_ORG_DOWNLOAD_HREF_RE = new Regex(@"nginx.*?(?<version>\d+(?:\.\d+){1,3}).*\.zip$", RegexOptions.IgnoreCase);
        private static readonly Regex NGINX_CLI_VERSION_RE = new Regex(@"^nginx version. \w+?/(?<version>\d+(?:\.\d+){1,3})\s*$", RegexOptions.IgnoreCase);

        public Core Core { get; }

        public NginxManagerState State { get; }

        public NginxManager(Core core)
        {
            Core = core;
            State = new NginxManagerState();
            State.PropertyChanged += State_Changed;
        }

        private void State_Changed(object sender, PropertyChangedEventArgs e)
        {
            if (
                e.PropertyName == nameof(NginxManagerState.ExecutableAvailable) ||
                e.PropertyName == nameof(NginxManagerState.SelectedExecutablePath)
            ) return;

            State.ExecutableAvailable = State.FoundSystemExecutable || State.FoundInternalExecutable;
            State.SelectedExecutablePath = State.FoundInternalExecutable
                ? InternalExecutablePath
                : State.FoundSystemExecutable
                    ? State.SystemPath
                    : null;
        }

        public Task UpdateState() => Task.WhenAll(FindSystemExecutable(), FindInternalExecutable());

        private async Task<string> AskNginxVersion(string executablePath)
        {
            var pi = new ProcessStartInfo(executablePath, "-v")
            {
                UseShellExecute = false,
                RedirectStandardOutput = false,
                RedirectStandardError = true,
                StandardErrorEncoding = Encoding.UTF8,
                CreateNoWindow = true,
            };
            var p = Process.Start(pi);
            var result = default(string);
            var line = default(string);
            while ((line = await p.StandardError.ReadLineAsync()) != null)
            {
                var m = NGINX_CLI_VERSION_RE.Match(line);
                if (m.Success)
                {
                    result = m.Groups["version"].Value;
                    break;
                }
            }
            await Task.Run(p.WaitForExit);
            return result;
        }

        public async Task FindSystemExecutable()
        {
            State.CheckingSystemExecutable = true;
            State.SystemPath = null;
            State.SystemVersion = null;
            State.FoundSystemExecutable = false;
            try
            {
                var path = await Task.Run(() => Environment.GetEnvironmentVariable("PATH")
                    .Split(Path.PathSeparator)
                    .Select(p => Path.Combine(p, "nginx.exe"))
                    .FirstOrDefault(f => File.Exists(f)));
                await Task.Run(() => System.Threading.Thread.Sleep(5000));
                if (path != null)
                {
                    State.SystemPath = path;
                    State.FoundSystemExecutable = true;
                    State.SystemVersion = await AskNginxVersion(path) ?? "unknown";
                }
                else
                {
                    State.SystemVersion = "not found";
                }
            }
            finally
            {
                State.CheckingSystemExecutable = false;
            }
        }

        private class OnlineVersion
        {
            public string Url { get; }
            public string Version { get; }

            public OnlineVersion(string url, string version)
            {
                Url = url;
                Version = version;
            }
        }

        private static OnlineVersion FindLatestOnlineVersion(HtmlDocument doc)
        {
            var baseUri = new Uri(NGINX_ORG_DOWNLOAD_URL);
            var versions = new List<OnlineVersion>();
            foreach (var n in doc.DocumentNode.SelectNodes(NGINX_ORG_DOWNLOAD_XPATH))
            {
                if (NGINX_ORG_DOWNLOAD_RE.IsMatch(n.InnerText))
                {
                    var href = n.GetAttributeValue("href", null);
                    var m = NGINX_ORG_DOWNLOAD_HREF_RE.Match(href);
                    if (m.Success)
                    {
                        var hrefUri = new Uri(href, UriKind.RelativeOrAbsolute);
                        if (!hrefUri.IsAbsoluteUri)
                        {
                            href = new Uri(baseUri, hrefUri).AbsoluteUri;
                        }
                        versions.Add(new OnlineVersion(href, m.Groups["version"].Value));
                    }
                }
            }
            if (versions.Count == 0) return null;
            versions.Sort((a, b) => (new Version(b.Version)).CompareTo(new Version(a.Version)));
            return versions[0];
        }

        public async Task FindOnlineExecutable()
        {
            State.CheckingOnlineExecutable = true;
            State.FoundOnlineExecutable = false;
            State.OnlineExecutableUrl = null;
            State.OnlineVersion = null;
            byte[] pageData;
            var doc = new HtmlDocument();
            try
            {
                using (var wc = new WebClient())
                {
                    pageData = await wc.DownloadDataTaskAsync(NGINX_ORG_DOWNLOAD_URL);
                }
                using (var ms = new MemoryStream(pageData))
                {
                    await Task.Run(() => doc.Load(ms));
                }
                var latestOnlineVersion = await Task.Run(() => FindLatestOnlineVersion(doc));
                if (latestOnlineVersion != null)
                {
                    State.FoundOnlineExecutable = true;
                    State.OnlineExecutableUrl = latestOnlineVersion.Url;
                    State.OnlineVersion = latestOnlineVersion.Version;
                }
            }
            finally
            {
                State.CheckingOnlineExecutable = false;
            }
        }

        private string ResourcePath => Path.Combine(Core.AbsoluteResourceDirectory, "nginx.zip");

        private string InternalExecutablePath => Path.Combine(Core.AbsoluteBinaryDirectory, "nginx.exe");

        public async Task DownloadOnlineExecutable()
        {
            if (string.IsNullOrWhiteSpace(State.OnlineVersion) ||
                string.IsNullOrWhiteSpace(State.OnlineExecutableUrl))
            {
                throw new InvalidOperationException("No URL for online executable available.");
            }
            State.DownloadingOnlineExecutable = true;
            State.FoundResourceExecutable = false;

            try
            {
                if (!Directory.Exists(Core.AbsoluteResourceDirectory))
                    Directory.CreateDirectory(Core.AbsoluteResourceDirectory);
                if (File.Exists(ResourcePath))
                {
                    File.Delete(ResourcePath);
                }
                using (var wc = new WebClient())
                {
                    await wc.DownloadFileTaskAsync(State.OnlineExecutableUrl, ResourcePath);
                }
                State.FoundResourceExecutable = File.Exists(ResourcePath);
                await FindInternalExecutable();
            }
            finally
            {
                State.DownloadingOnlineExecutable = false;
            }
        }

        public async Task ExtractOnlineExecutable()
        {
            State.ExtractingOnlineExecutable = true;
            State.FoundInternalExecutable = false;
            try
            {
                await Task.Run(() =>
                {
                    if (!Directory.Exists(Core.AbsoluteBinaryDirectory))
                        Directory.CreateDirectory(Core.AbsoluteBinaryDirectory);

                    if (!File.Exists(ResourcePath))
                    {
                        throw new InvalidOperationException("The ZIP file with the nginx executable is not present.");
                    }
                    using (var archive = ZipFile.Open(ResourcePath, ZipArchiveMode.Read))
                    {
                        var exeEntry = archive.Entries.FirstOrDefault(e => string.Equals(e.Name, "nginx.exe"));
                        if (exeEntry != null)
                        {
                            exeEntry.ExtractToFile(InternalExecutablePath, true);
                        }
                    }
                });
            }
            finally
            {
                State.ExtractingOnlineExecutable = false;
            }
        }

        public async Task FindInternalExecutable()
        {
            State.CheckingInternalExecutable = true;
            State.FoundInternalExecutable = false;
            State.InternalVersion = null;
            try
            {
                State.FoundInternalExecutable = await Task.Run(() => File.Exists(InternalExecutablePath));
                if (State.FoundInternalExecutable)
                {
                    State.InternalVersion = await AskNginxVersion(InternalExecutablePath) ?? "unknown";
                }
                else
                {
                    State.InternalVersion = "not found";
                }
            }
            finally
            {
                State.CheckingInternalExecutable = false;
            }
        }

        private static async Task WriteResourceFile(string targetFile, string name)
        {
            using (var src = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(NginxManager), "res." + name))
            {
                using (var trg = File.OpenWrite(targetFile))
                {
                    await src.CopyToAsync(trg);
                }
            }
        }

        public async Task SetupConfigDirectory(string configDir, string logDir, string certDir, Setup setup)
        {
            await Task.Run(() =>
            {
                if (!Directory.Exists(configDir)) Directory.CreateDirectory(configDir);
                var tempDir = Path.Combine(configDir, "temp");
                if (!Directory.Exists(tempDir)) Directory.CreateDirectory(tempDir);
                if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);
                if (setup.Server.UseHttps)
                {
                    if (!Directory.Exists(certDir)) Directory.CreateDirectory(certDir);
                }
            });
            await setup.WriteNginxConfig(Path.Combine(configDir, "nginx.conf"));
            await WriteResourceFile(Path.Combine(configDir, "mime.types"), "mime.types");
        }

        public async Task CheckConfigDirectory(string configDir)
        {
            if (State.SelectedExecutablePath == null)
            {
                throw new InvalidOperationException("No nginx executable selected.");
            }
            var configFilePath = Path.Combine(configDir, "nginx.conf");
            var pi = new ProcessStartInfo(State.SelectedExecutablePath, $"-t -q -c \"{configFilePath}\"")
            {
                WorkingDirectory = configDir,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                StandardErrorEncoding = Encoding.UTF8,
            };
            var p = Process.Start(pi);
            var errors = new List<string>();
            var line = default(string);
            while ((line = await p.StandardError.ReadLineAsync()) != null)
            {
                errors.Add(line);
            }
            await Task.Run(p.WaitForExit);
            State.ConfigurationErrors =
                errors.Count > 0 ? string.Join(Environment.NewLine, errors) : null;
            State.IsConfigurationValid = p.ExitCode == 0;
        }
    }
}
