using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using static Mastersign.Gate.NginxConfHelper;

namespace Mastersign.Gate
{
    partial class Server
    {
        private void Initialize()
        {
            HttpsCertificate = new Certificate();
        }

        public IEnumerable<string> NginxConfig(string certificateDirectory)
        {
            return Block("server",
                Chain(
                    // Setting("server_name", Environment.GetEnvironmentVariable("COMPUTERNAME")),

                    UseHttp
                        ? Setting("listen", $"{Host}:{HttpPort}")
                        : NoLines(),
                    UseHttps
                        ? Chain(
                            Setting("listen", $"{Host}:{HttpsPort}", "ssl"),
                            Setting("ssl_certificate", FsPath(certificateDirectory, "self-signed.pem")),
                            Setting("ssl_certificate_key", FsPath(certificateDirectory, "self-signed.key")),
                            Setting("ssl_protocols", "TLSv1", "TLSv1.1", "TLSv1.2"),
                            Setting("ssl_ciphers", "HIGH:!aNULL:!MD5")
                            )
                        : NoLines(),

                    Chain(Services.Select(s => s.NginxConfig()))
                )
            );
        }

        [YamlIgnore]
        public bool RequireWebSocketSupport => Services.Any(s => s.SupportWebSockets);
    }
}
