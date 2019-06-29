using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastersign.Gate
{

    public class DesignTimeCore : Core
    {
        public DesignTimeCore()
        {
            Setup = new DesignTimeSetup();
            NginxManager = new DesignTimeNginxManager();
        }
    }

    public class DesignTimeSetup : Setup
    {
        public DesignTimeSetup()
        {
            Server = new DesignTimeServer();
        }
    }

    public class DesignTimeServer : Server
    {
        public DesignTimeServer()
        {
            HttpsCertificate = new DesignTimeCertificate();
            Services.Add(new DesignTimeService { Name = "API", Route = "/api/", Url = "http://localhost:8081", SupportWebSockets = true });
            Services.Add(new DesignTimeService { Name = "Web", Route = "/", Url = "http://localhost:8080" });
        }
    }

    public class DesignTimeCertificate : Certificate
    {
        public DesignTimeCertificate()
        {
            Country = "DE";
            State = "Nordrhein-Westfahlen";
            Location = "Düsseldorf";
            Organization = "Das Unternehmen GmbH";
            OrganizationalUnit = "Verkauf";
            CommonName = "shop.das-unternehmen.de";
        }
    }

    public class DesignTimeService : Service
    {
        public DesignTimeService()
        {
            UrlRewrite = true;
            HtmlContentRewrite = true;
        }
    }

    public class DesignTimeNginxManager : NginxManager
    {
        public DesignTimeNginxManager()
            : base(null)
        {
            MonitorState.FoundSystemExecutable = false;
            MonitorState.SystemVersion = "not found";
            MonitorState.CheckingInternalExecutable = true;
            MonitorState.FoundOnlineExecutable = true;
            MonitorState.OnlineExecutableUrl = "https://nginx.org/download/nginx-1.16.1.zip";
            MonitorState.OnlineVersion = "1.16.1";
        }
    }
}
