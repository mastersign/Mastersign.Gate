using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using static Mastersign.Gate.NginxConfHelper;

namespace Mastersign.Gate
{
    partial class Service
    {
        public IEnumerable<string> NginxConfig()
            => Block("location",
                IsProxy
                    ? Chain(
                        Setting("set", "$backend", Url),
                        Setting("proxy_pass", "$backend"),
                        UrlRewriteConfig(),
                        ContentRewriteConfig(),
                        HeadersConfig(),
                        SupportWebSocketsConfig()
                      )
                    : Chain(
                        Setting("alias", FsPath(SafeTargetDirectory)),
                        string.IsNullOrWhiteSpace(IndexFiles) ? NoLines() : Setting("index", IndexFiles)
                      ),
                Route);

        private IEnumerable<string> UrlRewriteConfig()
            => UrlRewrite
                ? Setting("rewrite", $"^/{Route.Trim('/')}(/.*)$ $1 break")
                : NoLines();

        private IEnumerable<string> ContentRewriteConfig()
            => ContentRewrite
                ? Chain(
                    Setting("proxy_set_header", "Accept-Encoding", "\"\""),
                    Setting("sub_filter", $"'{Url}'", $"'$scheme://$host{AdaptTrailingSlash(Url, Route)}'"),
                    Setting("sub_filter_last_modified", "on"),
                    Setting("sub_filter_once", "off"),
                    ContentRewriteTypes().Any()
                        ? Setting("sub_filter_types", ContentRewriteTypes().ToArray())
                        : NoLines()
                    )
                : NoLines();

        private IEnumerable<string> HeadersConfig()
            => Chain(
                HeaderXForwardedFor
                    ? Setting("proxy_set_header", "X-Forwarded-For", "$proxy_add_x_forwarded_for")
                    : NoLines()
            );

        private IEnumerable<string> SupportWebSocketsConfig()
            => SupportWebSockets
                ? Chain(
                    Setting("proxy_http_version", "1.1"),
                    Setting("proxy_set_header", "Upgrade", "$http_upgrade"),
                    Setting("proxy_set_header", "Connection", "$connection_upgrade")
                    )
                : NoLines();

        private string AdaptTrailingSlash(string template, string value)
            => template.EndsWith("/")
                ? value.TrimEnd('/') + "/"
                : value.TrimEnd('/');

        [YamlIgnore]
        public bool ContentRewrite => HtmlContentRewrite || CssContentRewrite || JavaScriptContentRewrite;

        private IEnumerable<string> ContentRewriteTypes()
        {
            // yield return "*";
            // if (HtmlContentRewrite) yield return "text/html";
            if (CssContentRewrite) yield return "text/css";
            if (JavaScriptContentRewrite) yield return "application/javascript";
        }

        private string SafeTargetDirectory
        {
            get
            {
                var targetDir = TargetDirectory;
                if (!targetDir.EndsWith(new string(System.IO.Path.DirectorySeparatorChar, 1)) &&
                    !targetDir.EndsWith(new string(System.IO.Path.AltDirectorySeparatorChar, 1)))
                {
                    targetDir += System.IO.Path.DirectorySeparatorChar;
                }
                return targetDir;
            }
        }
    }
}
