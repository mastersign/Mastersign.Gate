using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private const string NGINX_ORG_DOWNLOAD_PATTERN = @"nginx.*Windows.*?(?<version>\d+(?:\.\d+){1,3})";
        private const string NGINX_ORG_DOWNLOAD_HREF_PATTERN = @"nginx.*?(?<version>\d+(?:\.\d+){1,3}).*\.zip$";

        private static readonly Regex NGINX_ORG_DOWNLOAD_RE = new Regex(NGINX_ORG_DOWNLOAD_PATTERN);
        private static readonly Regex NGINX_ORG_DOWNLOAD_HREF_RE = new Regex(NGINX_ORG_DOWNLOAD_HREF_PATTERN);

        public Core Core { get; }

        public NginxMonitorState MonitorState { get; }

        public NginxManager(Core core)
        {
            Core = core;
            MonitorState = new NginxMonitorState();
        }

        public async Task FindSystemExecutable()
        {
            MonitorState.CheckingSystemExecutable = true;
            try
            {
                var path = Environment.GetEnvironmentVariable("PATH")
                    .Split(Path.PathSeparator)
                    .Select(p => Path.Combine(p, "nginx.exe"))
                    .FirstOrDefault(f => File.Exists(f));
                if (path == null)
                {
                    MonitorState.FoundSystemExecutable = false;
                    MonitorState.SystemPath = path;
                    MonitorState.SystemVersion = null;
                }
                else
                {
                    MonitorState.FoundSystemExecutable = true;
                    MonitorState.SystemPath = null;
                    MonitorState.SystemVersion = null;
                    var pi = new ProcessStartInfo(path, "--version")
                    {
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        StandardOutputEncoding = Encoding.UTF8,
                        CreateNoWindow = true,
                    };
                    var p = Process.Start(pi);
                    MonitorState.SystemVersion = await p.StandardOutput.ReadLineAsync();
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
                    doc.Load(ms);
                }
                var latestOnlineVersion = FindLatestOnlineVersion(doc);
                if (latestOnlineVersion == null)
                {
                    MonitorState.FoundOnlineExecutable = false;
                    MonitorState.OnlineExecutableUrl = null;
                    MonitorState.OnlineVersion = null;
                }
                else
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

        public async Task DownloadOnlineExecutable()
        {
            if (string.IsNullOrWhiteSpace(MonitorState.OnlineVersion) ||
                string.IsNullOrWhiteSpace(MonitorState.OnlineExecutableUrl))
            {
                throw new InvalidOperationException("No URL for online executable available.");
            }
            if (!Directory.Exists(Core.AbsoluteResourceDirectory))
                Directory.CreateDirectory(Core.AbsoluteResourceDirectory);
            var resourceName = $"nginx.zip";
            var resourcePath = Path.Combine(Core.AbsoluteResourceDirectory, resourceName);

            MonitorState.DownloadingOnlineExecutable = true;

            try
            {
                using (var wc = new WebClient())
                {
                    await wc.DownloadFileTaskAsync(MonitorState.OnlineExecutableUrl, resourcePath);
                }
                await FindInternalExecutable();
            }
            finally
            {
                MonitorState.DownloadingOnlineExecutable = false;
            }
        }

        public async Task ExtractOnlineExecutable()
        {

        }

        public async Task FindInternalExecutable()
        {

        }
    }
}
