using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Mastersign.Gate.NginxConfHelper;

namespace Mastersign.Gate
{
    partial class Service
    {
        public IEnumerable<string> NginxConfig()
        {
            return Block("location",
                IsProxy
                    ? Chain(
                        Setting("set", "$backend", Url),
                        Setting("proxy_pass", "$backend"),
                        HeaderXForwardedFor
                            ? Setting("proxy_set_header", "X-Forwarded-For", "$proxy_add_x_forwarded_for")
                            : NoLines(),
                        SupportWebSockets
                            ? Chain(
                                Setting("proxy_http_version", "1.1"),
                                Setting("proxy_set_header", "Upgrade", "$http_upgrade"),
                                Setting("proxy_set_header", "Connection", "$connection_upgrade")
                                )
                            : NoLines()
                      )
                    : Chain(
                        Setting("alias", FsPath(SafeTargetDirectory)),
                        string.IsNullOrWhiteSpace(IndexFiles) ? NoLines() : Setting("index", IndexFiles)
                      ),
                Route);
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
