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
            State.IsServerRunningChanged += ServerRunningStateChanged;
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
            catch (UnauthorizedAccessException)
            {
                State.InternalVersion = "access denied";
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

        public async Task SetupConfigDirectory()
        {
            var configDir = Core.AbsoluteConfigDirectory;
            var logDir = Core.AbsoluteConfigPath(Core.Setup.LogDirectory);
            var certDir = Core.AbsoluteConfigPath(Core.Setup.CertificateDirectory);
            await Task.Run(() =>
            {
                if (!Directory.Exists(configDir)) Directory.CreateDirectory(configDir);
                var tempDir = Path.Combine(configDir, "temp");
                if (!Directory.Exists(tempDir)) Directory.CreateDirectory(tempDir);
                if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);
                
                if (Core.Setup.Server.UseHttps)
                {
                    if (!Directory.Exists(certDir)) Directory.CreateDirectory(certDir);
                    var certFile = Path.Combine(certDir, "self-signed.pem");
                    var keyFile = Path.Combine(certDir, "self-signed.key");
                    if (!File.Exists(certFile) || !File.Exists(keyFile))
                    {
                        CertificateBuilder.CreateSelfSignedCertificate(
                            Core.Setup.Server.HttpsCertificate, certFile, keyFile);
                    }
                }
            });
            await WriteResourceFile(Path.Combine(configDir, "mime.types"), "mime.types");
            await WriteWrapperScripts();
            await PrepareDefaultWwwRoot();
            await WriteNginxConfig();
            State.IsConfigurationReady = true;
        }

        private async Task WriteNginxConfig()
        {
            using (var w = new StreamWriter(Core.AbsoluteConfigPath(Core.Setup.ConfigFile), false, new UTF8Encoding(false)))
            {
                foreach (var line in Core.Setup.NginxConfig())
                {
                    await w.WriteLineAsync(line);
                }
            }
        }

        private async Task PrepareDefaultWwwRoot()
        {
            var configDir = Core.AbsoluteConfigDirectory;
            var wwwDir = Path.Combine(configDir, "www");
            await Task.Run(() =>
            {
                if (!Directory.Exists(wwwDir))
                {
                    Directory.CreateDirectory(wwwDir);
                    File.WriteAllText(Path.Combine(wwwDir, "index.html"),
                        "<!DOCTYPE html>" + Environment.NewLine +
                        "<html lang=\"en\">" + Environment.NewLine +
                        "<head>" + Environment.NewLine +
                        "  <meta charset=\"UTF-8\">" + Environment.NewLine +
                        "  <title>Mastersign Gate</title>" + Environment.NewLine +
                        "</head>" + Environment.NewLine +
                        "<body>" + Environment.NewLine +
                        "  <h1>Mastersign Gate</h1>" + Environment.NewLine +
                        "</body>" + Environment.NewLine +
                        "</html>",
                        new UTF8Encoding(false));
                }
            });
        }

        private async Task WriteWrapperScripts()
        {
            var configDir = Core.AbsoluteConfigDirectory;
            await Task.Run(() =>
            {
                File.WriteAllText(
                    Path.Combine(configDir, "start.cmd"),
                    $"@\"{State.SelectedExecutablePath}\" -c \"%~dp0{Core.Setup.ConfigFile}\"");
                File.WriteAllText(
                    Path.Combine(configDir, "reload.cmd"),
                    $"@\"{State.SelectedExecutablePath}\" -c \"%~dp0{Core.Setup.ConfigFile}\" -s reload");
                File.WriteAllText(
                    Path.Combine(configDir, "stop.cmd"),
                    $"@\"{State.SelectedExecutablePath}\" -c \"%~dp0{Core.Setup.ConfigFile}\" -s stop");
            });
        }

        public async Task<bool> CheckConfigDirectory()
        {
            if (State.SelectedExecutablePath == null)
            {
                throw new InvalidOperationException("No nginx executable selected.");
            }
            var configFilePath = Core.AbsoluteConfigPath("nginx.conf");
            var pi = new ProcessStartInfo(State.SelectedExecutablePath, $"-t -q -c \"{configFilePath}\"")
            {
                WorkingDirectory = Core.AbsoluteConfigDirectory,
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
            var valid = p.ExitCode == 0;
            State.IsConfigurationValid = valid;
            return valid;
        }

        public async Task<bool> ReloadServerConfig()
        {
            var p = RunNginx("-s reload");
            await Task.Run(p.WaitForExit);
            return p.ExitCode == 0;
        }

        private Process nginxServer;
        private bool stopping;

        private void ServerRunningStateChanged(object sender, EventArgs e)
        {
            if (State.IsServerRunning && nginxServer == null)
                _ = StartServer();
            else if (!State.IsServerRunning && nginxServer != null)
                StopServer();
        }

        private Process RunNginx(string args = null)
        {
            if (State.SelectedExecutablePath == null)
            {
                throw new InvalidOperationException("No nginx executable selected.");
            }
            var configFilePath = Core.AbsoluteConfigPath(Core.Setup.ConfigFile);
            args = $"-c \"{configFilePath}\"" + (args != null ? " " + args : string.Empty);
            var si = new ProcessStartInfo(State.SelectedExecutablePath, args)
            {
                WorkingDirectory = Core.AbsoluteConfigDirectory,
                CreateNoWindow = true,
                UseShellExecute = false,
            };
            return Process.Start(si);
        }

        private static string AccessableFrontendHost(Server server)
            => string.IsNullOrWhiteSpace(server.Host) || server.Host == "*" || server.Host == "0.0.0.0"
                ? "localhost"
                : server.Host;

        private void UpdateFrontendUrls()
        {
            var server = Core.Setup.Server;
            if (server.UseHttp)
            {
                State.HttpFrontendUrl = "http://"
                    + AccessableFrontendHost(server)
                    + (server.HttpPort == 80 ? string.Empty : ":" + server.HttpPort)
                    + "/";
            }
            if (server.UseHttps)
            {
                State.HttpsFrontendUrl = "https://"
                    + AccessableFrontendHost(server)
                    + (server.HttpsPort == 443 ? string.Empty : ":" + server.HttpsPort)
                    + "/";
            }
        }

        public async void FindRunningServer()
        {
            if (State.SelectedExecutablePath == null) return;
            if (nginxServer != null) return;
            var pidFile = Core.AbsoluteConfigPath(Core.Setup.ProcessIdFile);
            int pid = -1;
            await Task.Run(() =>
            {
                if (File.Exists(pidFile))
                {
                    var pidText = File.ReadAllText(pidFile);
                    if (int.TryParse(pidText, out pid))
                    {
                        var proc = Process.GetProcessById(pid);
                        nginxServer = proc;
                        State.IsServerRunning = true;
                        TrackExit();
                    }
                }
            });
        }

        public async Task StartServer()
        {
            if (State.SelectedExecutablePath == null) return;
            if (nginxServer != null) return;
            stopping = false;
            State.IsConfigurationReady = false;
            State.IsConfigurationValid = false;
            try
            {
                await SetupConfigDirectory();
            }
            catch (Exception ex)
            {
                State.ConfigurationErrors = ex.RecursiveMessage();
                return;
            }
            if (!await CheckConfigDirectory()) return;

            UpdateFrontendUrls();
            nginxServer = RunNginx();
            TrackExit();
        }

        private async void TrackExit()
        {
            while (nginxServer != null && !nginxServer.HasExited)
            {
                await Task.Delay(500);
            }
            StopServer();
        }

        public async void StopServer()
        {
            if (nginxServer == null) return;
            if (stopping) return;
            stopping = true;
            // signal nginx master process via inter-process-call
            RunNginx("-s stop");
            var exited = await Task.Run(() => nginxServer.WaitForExit(5000));
            if (!exited)
            {
                try
                {
                    // kill the master process hard
                    // worker processes are not covered!
                    nginxServer.Kill();
                }
                catch (Win32Exception)
                {
                    // process is currently exiting
                }
                catch (InvalidOperationException)
                {
                    // process has already exited
                }
                await Task.Run(nginxServer.WaitForExit);
            }
            nginxServer = null;
            stopping = false;
            State.IsServerRunning = false;
            State.HttpFrontendUrl = null;
            State.HttpsFrontendUrl = null;
            State.IsConfigurationReady = false;
            State.IsConfigurationValid = false;
        }

    }
}
