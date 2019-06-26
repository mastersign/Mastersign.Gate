using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Mastersign.Gate
{
    #region Scaleton Model Designer generated code
    
    // Scaleton Version: 0.3.0
    
    public partial class Setup
    {
        public Setup()
        {
            this._certificateDirectory = DEF_CERTIFICATEDIRECTORY;
            this._logDirectory = DEF_LOGDIRECTORY;
        }
        
        #region Property Directory
        
        private string _directory;
        
        public virtual string Directory
        {
            get { return _directory; }
            set
            {
                if (string.Equals(value, _directory))
                {
                    return;
                }
                _directory = value;
            }
        }
        
        #endregion
        
        #region Property CertificateDirectory
        
        private string _certificateDirectory;
        
        private const string DEF_CERTIFICATEDIRECTORY = @"certs";
        
        [DefaultValue(DEF_CERTIFICATEDIRECTORY)]
        public virtual string CertificateDirectory
        {
            get { return _certificateDirectory; }
            set
            {
                if (string.Equals(value, _certificateDirectory))
                {
                    return;
                }
                _certificateDirectory = value;
            }
        }
        
        #endregion
        
        #region Property LogDirectory
        
        private string _logDirectory;
        
        private const string DEF_LOGDIRECTORY = @"logs";
        
        [DefaultValue(DEF_LOGDIRECTORY)]
        public virtual string LogDirectory
        {
            get { return _logDirectory; }
            set
            {
                if (string.Equals(value, _logDirectory))
                {
                    return;
                }
                _logDirectory = value;
            }
        }
        
        #endregion
        
        #region Property Server
        
        private Server _server;
        
        public virtual Server Server
        {
            get { return _server; }
            set
            {
                if ((value == _server))
                {
                    return;
                }
                _server = value;
            }
        }
        
        #endregion
    }
    
    public partial class Server
    {
        public Server()
        {
            this._host = DEF_HOST;
            this._httpPort = DEF_HTTPPORT;
            this._httpsPort = DEF_HTTPSPORT;
            this._useHttp = DEF_USEHTTP;
            this._useHttps = DEF_USEHTTPS;
            this._services = new List<Service>();
        }
        
        #region Property Host
        
        private string _host;
        
        private const string DEF_HOST = @"0.0.0.0";
        
        [DefaultValue(DEF_HOST)]
        public virtual string Host
        {
            get { return _host; }
            set
            {
                if (string.Equals(value, _host))
                {
                    return;
                }
                _host = value;
            }
        }
        
        #endregion
        
        #region Property HttpPort
        
        private UInt16 _httpPort;
        
        private const UInt16 DEF_HTTPPORT = 80;
        
        [DefaultValue(DEF_HTTPPORT)]
        public virtual UInt16 HttpPort
        {
            get { return _httpPort; }
            set
            {
                if ((value == _httpPort))
                {
                    return;
                }
                _httpPort = value;
            }
        }
        
        #endregion
        
        #region Property HttpsPort
        
        private UInt16 _httpsPort;
        
        private const UInt16 DEF_HTTPSPORT = 443;
        
        [DefaultValue(DEF_HTTPSPORT)]
        public virtual UInt16 HttpsPort
        {
            get { return _httpsPort; }
            set
            {
                if ((value == _httpsPort))
                {
                    return;
                }
                _httpsPort = value;
            }
        }
        
        #endregion
        
        #region Property UseHttp
        
        private bool _useHttp;
        
        private const bool DEF_USEHTTP = false;
        
        [DefaultValue(DEF_USEHTTP)]
        public virtual bool UseHttp
        {
            get { return _useHttp; }
            set
            {
                if ((value == _useHttp))
                {
                    return;
                }
                _useHttp = value;
            }
        }
        
        #endregion
        
        #region Property UseHttps
        
        private bool _useHttps;
        
        private const bool DEF_USEHTTPS = true;
        
        [DefaultValue(DEF_USEHTTPS)]
        public virtual bool UseHttps
        {
            get { return _useHttps; }
            set
            {
                if ((value == _useHttps))
                {
                    return;
                }
                _useHttps = value;
            }
        }
        
        #endregion
        
        #region Property HttpsCertificate
        
        private Certificate _httpsCertificate;
        
        public virtual Certificate HttpsCertificate
        {
            get { return _httpsCertificate; }
            set
            {
                if ((value == _httpsCertificate))
                {
                    return;
                }
                _httpsCertificate = value;
            }
        }
        
        #endregion
        
        #region Property Services
        
        private List<Service> _services;
        
        public virtual List<Service> Services
        {
            get { return _services; }
            set
            {
                if ((value == _services))
                {
                    return;
                }
                _services = value;
            }
        }
        
        #endregion
    }
    
    public partial class Certificate
    {
        public Certificate()
        {
            this._commonName = DEF_COMMONNAME;
            this._validDays = DEF_VALIDDAYS;
            this._bits = DEF_BITS;
        }
        
        #region Property Country
        
        private string _country;
        
        public virtual string Country
        {
            get { return _country; }
            set
            {
                if (string.Equals(value, _country))
                {
                    return;
                }
                _country = value;
            }
        }
        
        #endregion
        
        #region Property State
        
        private string _state;
        
        public virtual string State
        {
            get { return _state; }
            set
            {
                if (string.Equals(value, _state))
                {
                    return;
                }
                _state = value;
            }
        }
        
        #endregion
        
        #region Property Location
        
        private string _location;
        
        public virtual string Location
        {
            get { return _location; }
            set
            {
                if (string.Equals(value, _location))
                {
                    return;
                }
                _location = value;
            }
        }
        
        #endregion
        
        #region Property Organization
        
        private string _organization;
        
        public virtual string Organization
        {
            get { return _organization; }
            set
            {
                if (string.Equals(value, _organization))
                {
                    return;
                }
                _organization = value;
            }
        }
        
        #endregion
        
        #region Property OrganizationalUnit
        
        private string _organizationalUnit;
        
        public virtual string OrganizationalUnit
        {
            get { return _organizationalUnit; }
            set
            {
                if (string.Equals(value, _organizationalUnit))
                {
                    return;
                }
                _organizationalUnit = value;
            }
        }
        
        #endregion
        
        #region Property CommonName
        
        private string _commonName;
        
        private const string DEF_COMMONNAME = @"localhost";
        
        [DefaultValue(DEF_COMMONNAME)]
        public virtual string CommonName
        {
            get { return _commonName; }
            set
            {
                if (string.Equals(value, _commonName))
                {
                    return;
                }
                _commonName = value;
            }
        }
        
        #endregion
        
        #region Property ValidDays
        
        private int _validDays;
        
        private const int DEF_VALIDDAYS = 365;
        
        [DefaultValue(DEF_VALIDDAYS)]
        public virtual int ValidDays
        {
            get { return _validDays; }
            set
            {
                if ((value == _validDays))
                {
                    return;
                }
                _validDays = value;
            }
        }
        
        #endregion
        
        #region Property Bits
        
        private int _bits;
        
        private const int DEF_BITS = 4096;
        
        [DefaultValue(DEF_BITS)]
        public virtual int Bits
        {
            get { return _bits; }
            set
            {
                if ((value == _bits))
                {
                    return;
                }
                _bits = value;
            }
        }
        
        #endregion
    }
    
    public partial class Service
    {
        public Service()
        {
            this._name = DEF_NAME;
            this._route = DEF_ROUTE;
            this._url = DEF_URL;
            this._headerXForwardedFor = DEF_HEADERXFORWARDEDFOR;
        }
        
        #region Property Name
        
        private string _name;
        
        private const string DEF_NAME = @"service";
        
        [DefaultValue(DEF_NAME)]
        public virtual string Name
        {
            get { return _name; }
            set
            {
                if (string.Equals(value, _name))
                {
                    return;
                }
                _name = value;
            }
        }
        
        #endregion
        
        #region Property Route
        
        private string _route;
        
        private const string DEF_ROUTE = @"/service/";
        
        [DefaultValue(DEF_ROUTE)]
        public virtual string Route
        {
            get { return _route; }
            set
            {
                if (string.Equals(value, _route))
                {
                    return;
                }
                _route = value;
            }
        }
        
        #endregion
        
        #region Property Url
        
        private string _url;
        
        private const string DEF_URL = @"http://127.0.0.1:8080/";
        
        [DefaultValue(DEF_URL)]
        public virtual string Url
        {
            get { return _url; }
            set
            {
                if (string.Equals(value, _url))
                {
                    return;
                }
                _url = value;
            }
        }
        
        #endregion
        
        #region Property SupportWebSockets
        
        private bool _supportWebSockets;
        
        public virtual bool SupportWebSockets
        {
            get { return _supportWebSockets; }
            set
            {
                if ((value == _supportWebSockets))
                {
                    return;
                }
                _supportWebSockets = value;
            }
        }
        
        #endregion
        
        #region Property UrlRewrite
        
        private bool _urlRewrite;
        
        public virtual bool UrlRewrite
        {
            get { return _urlRewrite; }
            set
            {
                if ((value == _urlRewrite))
                {
                    return;
                }
                _urlRewrite = value;
            }
        }
        
        #endregion
        
        #region Property HtmlContentRewrite
        
        private bool _htmlContentRewrite;
        
        public virtual bool HtmlContentRewrite
        {
            get { return _htmlContentRewrite; }
            set
            {
                if ((value == _htmlContentRewrite))
                {
                    return;
                }
                _htmlContentRewrite = value;
            }
        }
        
        #endregion
        
        #region Property CssContentRewrite
        
        private bool _cssContentRewrite;
        
        public virtual bool CssContentRewrite
        {
            get { return _cssContentRewrite; }
            set
            {
                if ((value == _cssContentRewrite))
                {
                    return;
                }
                _cssContentRewrite = value;
            }
        }
        
        #endregion
        
        #region Property JavaScriptContentRewrite
        
        private bool _javaScriptContentRewrite;
        
        public virtual bool JavaScriptContentRewrite
        {
            get { return _javaScriptContentRewrite; }
            set
            {
                if ((value == _javaScriptContentRewrite))
                {
                    return;
                }
                _javaScriptContentRewrite = value;
            }
        }
        
        #endregion
        
        #region Property HeaderXForwardedFor
        
        private bool _headerXForwardedFor;
        
        private const bool DEF_HEADERXFORWARDEDFOR = true;
        
        [DefaultValue(DEF_HEADERXFORWARDEDFOR)]
        public virtual bool HeaderXForwardedFor
        {
            get { return _headerXForwardedFor; }
            set
            {
                if ((value == _headerXForwardedFor))
                {
                    return;
                }
                _headerXForwardedFor = value;
            }
        }
        
        #endregion
    }
    
    #endregion
}