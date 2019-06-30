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
                Chain(
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
                ),
                Route);
        }
    }
}
