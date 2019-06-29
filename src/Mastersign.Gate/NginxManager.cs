using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
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
        private static readonly Regex NGINX_VERSION_RE = new Regex(@"^nginx version. \w+?/(?<version>\d+(?:\.\d+){1,3})\s*$", RegexOptions.IgnoreCase);

        public Core Core { get; }

        public NginxMonitorState MonitorState { get; }

        public NginxManager(Core core)
        {
            Core = core;
            MonitorState = new NginxMonitorState();
        }

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
                var m = NGINX_VERSION_RE.Match(line);
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
            MonitorState.CheckingSystemExecutable = true;
            MonitorState.SystemPath = null;
            MonitorState.SystemVersion = null;
            MonitorState.FoundSystemExecutable = false;
            try
            {
                var path = await Task.Run(() => Environment.GetEnvironmentVariable("PATH")
                    .Split(Path.PathSeparator)
                    .Select(p => Path.Combine(p, "nginx.exe"))
                    .FirstOrDefault(f => File.Exists(f)));
                await Task.Run(() => System.Threading.Thread.Sleep(5000));
                if (path != null)
                {
                    MonitorState.SystemPath = path;
                    MonitorState.FoundSystemExecutable = true;
                    MonitorState.SystemVersion = await AskNginxVersion(path) ?? "unknown";
                }
                else
                {
                    MonitorState.SystemVersion = "not found";
                }
            }
            finally
            {
                MonitorState.CheckingSystemExecutable = false;
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
            MonitorState.CheckingOnlineExecutable = true;
            MonitorState.FoundOnlineExecutable = false;
            MonitorState.OnlineExecutableUrl = null;
            MonitorState.OnlineVersion = null;
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
                    MonitorState.FoundOnlineExecutable = true;
                    MonitorState.OnlineExecutableUrl = latestOnlineVersion.Url;
                    MonitorState.OnlineVersion = latestOnlineVersion.Version;
                }
            }
            finally
            {
                MonitorState.CheckingOnlineExecutable = false;
            }
        }

        private string ResourceExecutablePath => Path.Combine(Core.AbsoluteResourceDirectory, "nginx.zip");

        private string InternalExecutablePath => Path.Combine(Core.AbsoluteBinaryDirectory, "nginx.exe");

        public async Task DownloadOnlineExecutable()
        {
            if (string.IsNullOrWhiteSpace(MonitorState.OnlineVersion) ||
                string.IsNullOrWhiteSpace(MonitorState.OnlineExecutableUrl))
            {
                throw new InvalidOperationException("No URL for online executable available.");
            }
            MonitorState.DownloadingOnlineExecutable = true;
            MonitorState.FoundResourceExecutable = false;

            try
            {
                if (!Directory.Exists(Core.AbsoluteResourceDirectory))
                    Directory.CreateDirectory(Core.AbsoluteResourceDirectory);
                if (File.Exists(ResourceExecutablePath))
                {
                    File.Delete(ResourceExecutablePath);
                }
                using (var wc = new WebClient())
                {
                    await wc.DownloadFileTaskAsync(MonitorState.OnlineExecutableUrl, ResourceExecutablePath);
                }
                MonitorState.FoundResourceExecutable = File.Exists(ResourceExecutablePath);
                await FindInternalExecutable();
            }
            finally
            {
                MonitorState.DownloadingOnlineExecutable = false;
            }
        }

        public async Task ExtractOnlineExecutable()
        {
            MonitorState.ExtractingOnlineExecutable = true;
            MonitorState.FoundInternalExecutable = false;
            try
            {
                await Task.Run(() =>
                {
                    if (!Directory.Exists(Core.AbsoluteBinaryDirectory))
                        Directory.CreateDirectory(Core.AbsoluteBinaryDirectory);

                    if (!File.Exists(ResourceExecutablePath))
                    {
                        throw new InvalidOperationException("The ZIP file with the nginx executable is not present.");
                    }
                    using (var archive = ZipFile.Open(ResourceExecutablePath, ZipArchiveMode.Read))
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
                MonitorState.ExtractingOnlineExecutable = false;
            }
        }

        public async Task FindInternalExecutable()
        {
            MonitorState.CheckingInternalExecutable = true;
            MonitorState.FoundInternalExecutable = false;
            MonitorState.InternalVersion = null;
            try
            {
                MonitorState.FoundInternalExecutable = await Task.Run(() => File.Exists(InternalExecutablePath));
                if (MonitorState.FoundInternalExecutable)
                {
                    MonitorState.InternalVersion = await AskNginxVersion(InternalExecutablePath);
                }
            }
            finally
            {
                MonitorState.CheckingInternalExecutable = false;
            }
        }
    }
}
