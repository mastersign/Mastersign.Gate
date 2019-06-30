using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using static Mastersign.Gate.NginxConfHelper;

namespace Mastersign.Gate
{
    partial class Setup
    {
        private void Initialize()
        {
            Version = ProjectFile<Setup>.CURRENT_VERSION;
            Server = new Server();
        }

        [YamlIgnore]
        public string ProcessIdFile => Path.Combine(LogDirectory, "nginx.pid");

        [YamlIgnore]
        public string ErrorLogFile => Path.Combine(LogDirectory, "error.log");

        [YamlIgnore]
        public string AccessLogFile => Path.Combine(LogDirectory, "access.log");

        public IEnumerable<string> NginxConfig()
        {
            return Chain(
                Setting("worker_processes", "1"),
                Setting("error_log", FsPath(ErrorLogFile)),
                Setting("pid", FsPath(ProcessIdFile)),
                Block("events",
                    Setting("worker_connections", "1024")
                ),
                Block("http",
                    Chain(
                        Setting("include", "mime.types"),
                        Setting("default_type", "application/octet-stream"),
                        Setting("access_log", FsPath(AccessLogFile)),
                        Setting("sendfile", "on"),
                        Setting("keepalive_timeout", "65"),
                        Server.RequireWebSocketSupport
                            ? Block("map",
                                Chain(
                                    Setting("default", "upgrade"),
                                    Setting("''", "close")
                                ),
                                "$http_upgrade", "$connection_upgrade")
                            : NoLines(),
                        Server.NginxConfig(CertificateDirectory)
                    )
                )
            );
        }

        public async Task WriteNginxConfig(string configFilePath)
        {
            using (var w = new StreamWriter(configFilePath, false, new UTF8Encoding(false)))
            {
                foreach (var line in NginxConfig())
                {
                    await w.WriteLineAsync(line);
                }
            }
        }
    }
}
