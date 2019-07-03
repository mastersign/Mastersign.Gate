using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Globalization;

namespace Mastersign.Gate
{
    #region Scaleton Model Designer generated code
    
    // Scaleton Version: 0.3.0
    
    public partial class Core : INotifyPropertyChanged, IChangeTracking
    {
        public Core()
        {
            this._resourceDirectory = DEF_RESOURCEDIRECTORY;
            this._binaryDirectory = DEF_BINARYDIRECTORY;
            this.Initialize();
            
            this.IsChanged = false;
        }
        
        [OnDeserialized]
        internal void AfterDeserializingCore(StreamingContext serializationContext)
        {
            if (!ReferenceEquals(_setup, null))
            {
                _setup.PropertyChanged += this.SetupPropertyChangedHandler;
            }
        }
        
        #region Change Tracking
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        
        private bool _isChanged = false;
        
        [Browsable(false)]
        [XmlIgnore]
        public bool IsChanged
        {
            get { return this._isChanged; }
            protected set
            {
                if ((this._isChanged == value))
                {
                    return;
                }
                this._isChanged = value;
                this.OnPropertyChanged(@"IsChanged");
            }
        }
        
        public virtual void AcceptChanges()
        {
            if (!ReferenceEquals(_setup, null))
            {
                _setup.AcceptChanges();
            }
            this.IsChanged = false;
        }
        
        #endregion
        
        #region Property ProjectFilePath
        
        private string _projectFilePath;
        
        public event EventHandler ProjectFilePathChanged;
        
        protected virtual void OnProjectFilePathChanged()
        {
            this.IsChanged = true;
            EventHandler handler = ProjectFilePathChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"ProjectFilePath");
        }
        
        public virtual string ProjectFilePath
        {
            get { return _projectFilePath; }
            set
            {
                if (string.Equals(value, _projectFilePath))
                {
                    return;
                }
                _projectFilePath = value;
                this.OnProjectFilePathChanged();
            }
        }
        
        #endregion
        
        #region Property ResourceDirectory
        
        private string _resourceDirectory;
        
        public event EventHandler ResourceDirectoryChanged;
        
        protected virtual void OnResourceDirectoryChanged()
        {
            this.IsChanged = true;
            EventHandler handler = ResourceDirectoryChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"ResourceDirectory");
        }
        
        private const string DEF_RESOURCEDIRECTORY = @"res";
        
        [DefaultValue(DEF_RESOURCEDIRECTORY)]
        public virtual string ResourceDirectory
        {
            get { return _resourceDirectory; }
            set
            {
                if (string.Equals(value, _resourceDirectory))
                {
                    return;
                }
                _resourceDirectory = value;
                this.OnResourceDirectoryChanged();
            }
        }
        
        #endregion
        
        #region Property BinaryDirectory
        
        private string _binaryDirectory;
        
        public event EventHandler BinaryDirectoryChanged;
        
        protected virtual void OnBinaryDirectoryChanged()
        {
            this.IsChanged = true;
            EventHandler handler = BinaryDirectoryChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"BinaryDirectory");
        }
        
        private const string DEF_BINARYDIRECTORY = @"lib";
        
        [DefaultValue(DEF_BINARYDIRECTORY)]
        public virtual string BinaryDirectory
        {
            get { return _binaryDirectory; }
            set
            {
                if (string.Equals(value, _binaryDirectory))
                {
                    return;
                }
                _binaryDirectory = value;
                this.OnBinaryDirectoryChanged();
            }
        }
        
        #endregion
        
        #region Property Setup
        
        private Setup _setup;
        
        public event EventHandler SetupChanged;
        
        protected virtual void OnSetupChanged()
        {
            this.IsChanged = true;
            EventHandler handler = SetupChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Setup");
        }
        
        private void SetupPropertyChangedHandler(object sender, PropertyChangedEventArgs ea)
        {
            if (!string.Equals(ea.PropertyName, @"IsChanged"))
            {
                this.OnSetupChanged();
            }
        }
        
        public virtual Setup Setup
        {
            get { return _setup; }
            set
            {
                if ((value == _setup))
                {
                    return;
                }
                if (!ReferenceEquals(_setup, null))
                {
                    _setup.PropertyChanged -= this.SetupPropertyChangedHandler;
                }
                _setup = value;
                if (!ReferenceEquals(_setup, null))
                {
                    _setup.PropertyChanged += this.SetupPropertyChangedHandler;
                }
                this.OnSetupChanged();
            }
        }
        
        #endregion
    }
    
    public partial class Setup : INotifyPropertyChanged, IChangeTracking
    {
        public Setup()
        {
            this._name = DEF_NAME;
            this._certificateDirectory = DEF_CERTIFICATEDIRECTORY;
            this._logDirectory = DEF_LOGDIRECTORY;
            this.Initialize();
            
            this.IsChanged = false;
        }
        
        [OnDeserialized]
        internal void AfterDeserializingSetup(StreamingContext serializationContext)
        {
            if (!ReferenceEquals(_server, null))
            {
                _server.PropertyChanged += this.ServerPropertyChangedHandler;
            }
        }
        
        #region Change Tracking
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        
        private bool _isChanged = false;
        
        [Browsable(false)]
        [XmlIgnore]
        public bool IsChanged
        {
            get { return this._isChanged; }
            protected set
            {
                if ((this._isChanged == value))
                {
                    return;
                }
                this._isChanged = value;
                this.OnPropertyChanged(@"IsChanged");
            }
        }
        
        public virtual void AcceptChanges()
        {
            if (!ReferenceEquals(_server, null))
            {
                _server.AcceptChanges();
            }
            this.IsChanged = false;
        }
        
        #endregion
        
        #region Property Version
        
        private string _version;
        
        public event EventHandler VersionChanged;
        
        protected virtual void OnVersionChanged()
        {
            this.IsChanged = true;
            EventHandler handler = VersionChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Version");
        }
        
        public virtual string Version
        {
            get { return _version; }
            set
            {
                if (string.Equals(value, _version))
                {
                    return;
                }
                _version = value;
                this.OnVersionChanged();
            }
        }
        
        #endregion
        
        #region Property Name
        
        private string _name;
        
        public event EventHandler NameChanged;
        
        protected virtual void OnNameChanged()
        {
            this.IsChanged = true;
            EventHandler handler = NameChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Name");
        }
        
        private const string DEF_NAME = @"Unknown";
        
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
                this.OnNameChanged();
            }
        }
        
        #endregion
        
        #region Property Directory
        
        private string _directory;
        
        public event EventHandler DirectoryChanged;
        
        protected virtual void OnDirectoryChanged()
        {
            this.IsChanged = true;
            EventHandler handler = DirectoryChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Directory");
        }
        
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
                this.OnDirectoryChanged();
            }
        }
        
        #endregion
        
        #region Property CertificateDirectory
        
        private string _certificateDirectory;
        
        public event EventHandler CertificateDirectoryChanged;
        
        protected virtual void OnCertificateDirectoryChanged()
        {
            this.IsChanged = true;
            EventHandler handler = CertificateDirectoryChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"CertificateDirectory");
        }
        
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
                this.OnCertificateDirectoryChanged();
            }
        }
        
        #endregion
        
        #region Property LogDirectory
        
        private string _logDirectory;
        
        public event EventHandler LogDirectoryChanged;
        
        protected virtual void OnLogDirectoryChanged()
        {
            this.IsChanged = true;
            EventHandler handler = LogDirectoryChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"LogDirectory");
        }
        
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
                this.OnLogDirectoryChanged();
            }
        }
        
        #endregion
        
        #region Property Server
        
        private Server _server;
        
        public event EventHandler ServerChanged;
        
        protected virtual void OnServerChanged()
        {
            this.IsChanged = true;
            EventHandler handler = ServerChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Server");
        }
        
        private void ServerPropertyChangedHandler(object sender, PropertyChangedEventArgs ea)
        {
            if (!string.Equals(ea.PropertyName, @"IsChanged"))
            {
                this.OnServerChanged();
            }
        }
        
        public virtual Server Server
        {
            get { return _server; }
            set
            {
                if ((value == _server))
                {
                    return;
                }
                if (!ReferenceEquals(_server, null))
                {
                    _server.PropertyChanged -= this.ServerPropertyChangedHandler;
                }
                _server = value;
                if (!ReferenceEquals(_server, null))
                {
                    _server.PropertyChanged += this.ServerPropertyChangedHandler;
                }
                this.OnServerChanged();
            }
        }
        
        #endregion
    }
    
    public partial class Server : INotifyPropertyChanged, IChangeTracking
    {
        public Server()
        {
            this._host = DEF_HOST;
            this._httpPort = DEF_HTTPPORT;
            this._httpsPort = DEF_HTTPSPORT;
            this._useHttp = DEF_USEHTTP;
            this._useHttps = DEF_USEHTTPS;
            this._services = new global::System.Collections.ObjectModel.ObservableCollection<Service>();
            this._rootDirectory = DEF_ROOTDIRECTORY;
            this._indexFiles = DEF_INDEXFILES;
            this.Initialize();
            
            this.IsChanged = false;
        }
        
        [OnDeserialized]
        internal void AfterDeserializingServer(StreamingContext serializationContext)
        {
            if (!ReferenceEquals(_httpsCertificate, null))
            {
                _httpsCertificate.PropertyChanged += this.HttpsCertificatePropertyChangedHandler;
            }
            if (!ReferenceEquals(_services, null))
            {
                _services.CollectionChanged += this.ServicesCollectionChangedHandler;
                foreach (Service item in _services)
                {
                    if (!ReferenceEquals(item, null))
                    {
                        item.PropertyChanged += this.ServicesItemPropertyChanged;
                    }
                }
            }
        }
        
        #region Change Tracking
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        
        private bool _isChanged = false;
        
        [Browsable(false)]
        [XmlIgnore]
        public bool IsChanged
        {
            get { return this._isChanged; }
            protected set
            {
                if ((this._isChanged == value))
                {
                    return;
                }
                this._isChanged = value;
                this.OnPropertyChanged(@"IsChanged");
            }
        }
        
        public virtual void AcceptChanges()
        {
            if (!ReferenceEquals(_httpsCertificate, null))
            {
                _httpsCertificate.AcceptChanges();
            }
            if (!ReferenceEquals(_services, null))
            {
                foreach (Service item in _services)
                {
                    if (!ReferenceEquals(item, null))
                    {
                        item.AcceptChanges();
                    }
                }
            }
            this.IsChanged = false;
        }
        
        #endregion
        
        #region Property Host
        
        private string _host;
        
        public event EventHandler HostChanged;
        
        protected virtual void OnHostChanged()
        {
            this.IsChanged = true;
            EventHandler handler = HostChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Host");
        }
        
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
                this.OnHostChanged();
            }
        }
        
        #endregion
        
        #region Property HttpPort
        
        private UInt16 _httpPort;
        
        public event EventHandler HttpPortChanged;
        
        protected virtual void OnHttpPortChanged()
        {
            this.IsChanged = true;
            EventHandler handler = HttpPortChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"HttpPort");
        }
        
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
                this.OnHttpPortChanged();
            }
        }
        
        #endregion
        
        #region Property HttpsPort
        
        private UInt16 _httpsPort;
        
        public event EventHandler HttpsPortChanged;
        
        protected virtual void OnHttpsPortChanged()
        {
            this.IsChanged = true;
            EventHandler handler = HttpsPortChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"HttpsPort");
        }
        
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
                this.OnHttpsPortChanged();
            }
        }
        
        #endregion
        
        #region Property UseHttp
        
        private bool _useHttp;
        
        public event EventHandler UseHttpChanged;
        
        protected virtual void OnUseHttpChanged()
        {
            this.IsChanged = true;
            EventHandler handler = UseHttpChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"UseHttp");
        }
        
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
                this.OnUseHttpChanged();
            }
        }
        
        #endregion
        
        #region Property UseHttps
        
        private bool _useHttps;
        
        public event EventHandler UseHttpsChanged;
        
        protected virtual void OnUseHttpsChanged()
        {
            this.IsChanged = true;
            EventHandler handler = UseHttpsChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"UseHttps");
        }
        
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
                this.OnUseHttpsChanged();
            }
        }
        
        #endregion
        
        #region Property HttpsCertificate
        
        private Certificate _httpsCertificate;
        
        public event EventHandler HttpsCertificateChanged;
        
        protected virtual void OnHttpsCertificateChanged()
        {
            this.IsChanged = true;
            EventHandler handler = HttpsCertificateChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"HttpsCertificate");
        }
        
        private void HttpsCertificatePropertyChangedHandler(object sender, PropertyChangedEventArgs ea)
        {
            if (!string.Equals(ea.PropertyName, @"IsChanged"))
            {
                this.OnHttpsCertificateChanged();
            }
        }
        
        public virtual Certificate HttpsCertificate
        {
            get { return _httpsCertificate; }
            set
            {
                if ((value == _httpsCertificate))
                {
                    return;
                }
                if (!ReferenceEquals(_httpsCertificate, null))
                {
                    _httpsCertificate.PropertyChanged -= this.HttpsCertificatePropertyChangedHandler;
                }
                _httpsCertificate = value;
                if (!ReferenceEquals(_httpsCertificate, null))
                {
                    _httpsCertificate.PropertyChanged += this.HttpsCertificatePropertyChangedHandler;
                }
                this.OnHttpsCertificateChanged();
            }
        }
        
        #endregion
        
        #region Property Services
        
        private global::System.Collections.ObjectModel.ObservableCollection<Service> _services;
        
        public event EventHandler ServicesChanged;
        
        protected virtual void OnServicesChanged()
        {
            this.IsChanged = true;
            EventHandler handler = ServicesChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Services");
        }
        
        private void ServicesItemPropertyChanged(object sender, EventArgs ea)
        {
            this.OnServicesChanged();
        }
        
        private void ServicesCollectionChangedHandler(object sender, global::System.Collections.Specialized.NotifyCollectionChangedEventArgs ea)
        {
            if (!ReferenceEquals(ea.OldItems, null))
            {
                foreach (Service item in ea.OldItems)
                {
                    if (!ReferenceEquals(item, null))
                    {
                        item.PropertyChanged -= this.ServicesItemPropertyChanged;
                    }
                }
            }
            this.OnServicesChanged();
            if (!ReferenceEquals(ea.NewItems, null))
            {
                foreach (Service item in ea.NewItems)
                {
                    if (!ReferenceEquals(item, null))
                    {
                        item.PropertyChanged += this.ServicesItemPropertyChanged;
                    }
                }
            }
        }
        
        public virtual global::System.Collections.ObjectModel.ObservableCollection<Service> Services
        {
            get { return _services; }
            set
            {
                if ((value == _services))
                {
                    return;
                }
                if (!ReferenceEquals(_services, null))
                {
                    _services.CollectionChanged -= this.ServicesCollectionChangedHandler;
                }
                _services = value;
                if (!ReferenceEquals(_services, null))
                {
                    _services.CollectionChanged += this.ServicesCollectionChangedHandler;
                }
                this.OnServicesChanged();
            }
        }
        
        #endregion
        
        #region Property RootDirectory
        
        private string _rootDirectory;
        
        public event EventHandler RootDirectoryChanged;
        
        protected virtual void OnRootDirectoryChanged()
        {
            this.IsChanged = true;
            EventHandler handler = RootDirectoryChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"RootDirectory");
        }
        
        private const string DEF_ROOTDIRECTORY = @"www";
        
        [DefaultValue(DEF_ROOTDIRECTORY)]
        public virtual string RootDirectory
        {
            get { return _rootDirectory; }
            set
            {
                if (string.Equals(value, _rootDirectory))
                {
                    return;
                }
                _rootDirectory = value;
                this.OnRootDirectoryChanged();
            }
        }
        
        #endregion
        
        #region Property IndexFiles
        
        private string _indexFiles;
        
        public event EventHandler IndexFilesChanged;
        
        protected virtual void OnIndexFilesChanged()
        {
            this.IsChanged = true;
            EventHandler handler = IndexFilesChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"IndexFiles");
        }
        
        private const string DEF_INDEXFILES = @"index.html index.htm default.html default.htm";
        
        [DefaultValue(DEF_INDEXFILES)]
        public virtual string IndexFiles
        {
            get { return _indexFiles; }
            set
            {
                if (string.Equals(value, _indexFiles))
                {
                    return;
                }
                _indexFiles = value;
                this.OnIndexFilesChanged();
            }
        }
        
        #endregion
    }
    
    public partial class Certificate : INotifyPropertyChanged, IChangeTracking
    {
        public Certificate()
        {
            this._commonName = DEF_COMMONNAME;
            this._validDays = DEF_VALIDDAYS;
            this._bits = DEF_BITS;
            
            this.IsChanged = false;
        }
        
        #region Change Tracking
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        
        private bool _isChanged = false;
        
        [Browsable(false)]
        [XmlIgnore]
        public bool IsChanged
        {
            get { return this._isChanged; }
            protected set
            {
                if ((this._isChanged == value))
                {
                    return;
                }
                this._isChanged = value;
                this.OnPropertyChanged(@"IsChanged");
            }
        }
        
        public virtual void AcceptChanges()
        {
            this.IsChanged = false;
        }
        
        #endregion
        
        #region Property Country
        
        private string _country;
        
        public event EventHandler CountryChanged;
        
        protected virtual void OnCountryChanged()
        {
            this.IsChanged = true;
            EventHandler handler = CountryChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Country");
        }
        
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
                this.OnCountryChanged();
            }
        }
        
        #endregion
        
        #region Property State
        
        private string _state;
        
        public event EventHandler StateChanged;
        
        protected virtual void OnStateChanged()
        {
            this.IsChanged = true;
            EventHandler handler = StateChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"State");
        }
        
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
                this.OnStateChanged();
            }
        }
        
        #endregion
        
        #region Property Location
        
        private string _location;
        
        public event EventHandler LocationChanged;
        
        protected virtual void OnLocationChanged()
        {
            this.IsChanged = true;
            EventHandler handler = LocationChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Location");
        }
        
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
                this.OnLocationChanged();
            }
        }
        
        #endregion
        
        #region Property Organization
        
        private string _organization;
        
        public event EventHandler OrganizationChanged;
        
        protected virtual void OnOrganizationChanged()
        {
            this.IsChanged = true;
            EventHandler handler = OrganizationChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Organization");
        }
        
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
                this.OnOrganizationChanged();
            }
        }
        
        #endregion
        
        #region Property OrganizationalUnit
        
        private string _organizationalUnit;
        
        public event EventHandler OrganizationalUnitChanged;
        
        protected virtual void OnOrganizationalUnitChanged()
        {
            this.IsChanged = true;
            EventHandler handler = OrganizationalUnitChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"OrganizationalUnit");
        }
        
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
                this.OnOrganizationalUnitChanged();
            }
        }
        
        #endregion
        
        #region Property CommonName
        
        private string _commonName;
        
        public event EventHandler CommonNameChanged;
        
        protected virtual void OnCommonNameChanged()
        {
            this.IsChanged = true;
            EventHandler handler = CommonNameChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"CommonName");
        }
        
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
                this.OnCommonNameChanged();
            }
        }
        
        #endregion
        
        #region Property ValidDays
        
        private int _validDays;
        
        public event EventHandler ValidDaysChanged;
        
        protected virtual void OnValidDaysChanged()
        {
            this.IsChanged = true;
            EventHandler handler = ValidDaysChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"ValidDays");
        }
        
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
                this.OnValidDaysChanged();
            }
        }
        
        #endregion
        
        #region Property Bits
        
        private int _bits;
        
        public event EventHandler BitsChanged;
        
        protected virtual void OnBitsChanged()
        {
            this.IsChanged = true;
            EventHandler handler = BitsChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Bits");
        }
        
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
                this.OnBitsChanged();
            }
        }
        
        #endregion
    }
    
    public partial class Service : INotifyPropertyChanged, IChangeTracking
    {
        public Service()
        {
            this._name = DEF_NAME;
            this._route = DEF_ROUTE;
            this._targetDirectory = DEF_TARGETDIRECTORY;
            this._url = DEF_URL;
            this._headerXForwardedFor = DEF_HEADERXFORWARDEDFOR;
            
            this.IsChanged = false;
        }
        
        #region Change Tracking
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        
        private bool _isChanged = false;
        
        [Browsable(false)]
        [XmlIgnore]
        public bool IsChanged
        {
            get { return this._isChanged; }
            protected set
            {
                if ((this._isChanged == value))
                {
                    return;
                }
                this._isChanged = value;
                this.OnPropertyChanged(@"IsChanged");
            }
        }
        
        public virtual void AcceptChanges()
        {
            this.IsChanged = false;
        }
        
        #endregion
        
        #region Property Name
        
        private string _name;
        
        public event EventHandler NameChanged;
        
        protected virtual void OnNameChanged()
        {
            this.IsChanged = true;
            EventHandler handler = NameChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Name");
        }
        
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
                this.OnNameChanged();
            }
        }
        
        #endregion
        
        #region Property Route
        
        private string _route;
        
        public event EventHandler RouteChanged;
        
        protected virtual void OnRouteChanged()
        {
            this.IsChanged = true;
            EventHandler handler = RouteChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Route");
        }
        
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
                this.OnRouteChanged();
            }
        }
        
        #endregion
        
        #region Property IsProxy
        
        private bool _isProxy;
        
        public event EventHandler IsProxyChanged;
        
        protected virtual void OnIsProxyChanged()
        {
            this.IsChanged = true;
            EventHandler handler = IsProxyChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"IsProxy");
        }
        
        public virtual bool IsProxy
        {
            get { return _isProxy; }
            set
            {
                if ((value == _isProxy))
                {
                    return;
                }
                _isProxy = value;
                this.OnIsProxyChanged();
            }
        }
        
        #endregion
        
        #region Property TargetDirectory
        
        private string _targetDirectory;
        
        public event EventHandler TargetDirectoryChanged;
        
        protected virtual void OnTargetDirectoryChanged()
        {
            this.IsChanged = true;
            EventHandler handler = TargetDirectoryChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"TargetDirectory");
        }
        
        private const string DEF_TARGETDIRECTORY = @"www";
        
        [DefaultValue(DEF_TARGETDIRECTORY)]
        public virtual string TargetDirectory
        {
            get { return _targetDirectory; }
            set
            {
                if (string.Equals(value, _targetDirectory))
                {
                    return;
                }
                _targetDirectory = value;
                this.OnTargetDirectoryChanged();
            }
        }
        
        #endregion
        
        #region Property IndexFiles
        
        private string _indexFiles;
        
        public event EventHandler IndexFilesChanged;
        
        protected virtual void OnIndexFilesChanged()
        {
            this.IsChanged = true;
            EventHandler handler = IndexFilesChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"IndexFiles");
        }
        
        public virtual string IndexFiles
        {
            get { return _indexFiles; }
            set
            {
                if (string.Equals(value, _indexFiles))
                {
                    return;
                }
                _indexFiles = value;
                this.OnIndexFilesChanged();
            }
        }
        
        #endregion
        
        #region Property Url
        
        private string _url;
        
        public event EventHandler UrlChanged;
        
        protected virtual void OnUrlChanged()
        {
            this.IsChanged = true;
            EventHandler handler = UrlChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"Url");
        }
        
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
                this.OnUrlChanged();
            }
        }
        
        #endregion
        
        #region Property SupportWebSockets
        
        private bool _supportWebSockets;
        
        public event EventHandler SupportWebSocketsChanged;
        
        protected virtual void OnSupportWebSocketsChanged()
        {
            this.IsChanged = true;
            EventHandler handler = SupportWebSocketsChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"SupportWebSockets");
        }
        
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
                this.OnSupportWebSocketsChanged();
            }
        }
        
        #endregion
        
        #region Property UrlRewrite
        
        private bool _urlRewrite;
        
        public event EventHandler UrlRewriteChanged;
        
        protected virtual void OnUrlRewriteChanged()
        {
            this.IsChanged = true;
            EventHandler handler = UrlRewriteChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"UrlRewrite");
        }
        
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
                this.OnUrlRewriteChanged();
            }
        }
        
        #endregion
        
        #region Property HtmlContentRewrite
        
        private bool _htmlContentRewrite;
        
        public event EventHandler HtmlContentRewriteChanged;
        
        protected virtual void OnHtmlContentRewriteChanged()
        {
            this.IsChanged = true;
            EventHandler handler = HtmlContentRewriteChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"HtmlContentRewrite");
        }
        
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
                this.OnHtmlContentRewriteChanged();
            }
        }
        
        #endregion
        
        #region Property CssContentRewrite
        
        private bool _cssContentRewrite;
        
        public event EventHandler CssContentRewriteChanged;
        
        protected virtual void OnCssContentRewriteChanged()
        {
            this.IsChanged = true;
            EventHandler handler = CssContentRewriteChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"CssContentRewrite");
        }
        
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
                this.OnCssContentRewriteChanged();
            }
        }
        
        #endregion
        
        #region Property JavaScriptContentRewrite
        
        private bool _javaScriptContentRewrite;
        
        public event EventHandler JavaScriptContentRewriteChanged;
        
        protected virtual void OnJavaScriptContentRewriteChanged()
        {
            this.IsChanged = true;
            EventHandler handler = JavaScriptContentRewriteChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"JavaScriptContentRewrite");
        }
        
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
                this.OnJavaScriptContentRewriteChanged();
            }
        }
        
        #endregion
        
        #region Property HeaderXForwardedFor
        
        private bool _headerXForwardedFor;
        
        public event EventHandler HeaderXForwardedForChanged;
        
        protected virtual void OnHeaderXForwardedForChanged()
        {
            this.IsChanged = true;
            EventHandler handler = HeaderXForwardedForChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"HeaderXForwardedFor");
        }
        
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
                this.OnHeaderXForwardedForChanged();
            }
        }
        
        #endregion
    }
    
    #endregion
}