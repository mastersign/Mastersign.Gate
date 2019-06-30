using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Mastersign.Gate
{
    #region Scaleton Model Designer generated code
    
    // Scaleton Version: 0.3.0
    
    public partial class NginxManagerState : INotifyPropertyChanged
    {
        public NginxManagerState()
        {
        }
        
        #region String Representation
        
        public override string ToString()
        {
            return this.ToString(CultureInfo.CurrentUICulture);
        }
        
        public virtual string ToString(IFormatProvider formatProvider)
        {
            return (this.GetType().FullName + @": " + (
                (Environment.NewLine + @"    ExecutableAvailable = " + _executableAvailable.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    SelectedExecutablePath = " + (!ReferenceEquals(_selectedExecutablePath, null) ? _selectedExecutablePath.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    CheckingSystemExecutable = " + _checkingSystemExecutable.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    FoundSystemExecutable = " + _foundSystemExecutable.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    SystemPath = " + (!ReferenceEquals(_systemPath, null) ? _systemPath.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    SystemVersion = " + (!ReferenceEquals(_systemVersion, null) ? _systemVersion.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    CheckingOnlineExecutable = " + _checkingOnlineExecutable.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    FoundOnlineExecutable = " + _foundOnlineExecutable.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    OnlineVersion = " + (!ReferenceEquals(_onlineVersion, null) ? _onlineVersion.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    OnlineExecutableUrl = " + (!ReferenceEquals(_onlineExecutableUrl, null) ? _onlineExecutableUrl.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    DownloadingOnlineExecutable = " + _downloadingOnlineExecutable.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    FoundResourceExecutable = " + _foundResourceExecutable.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    ExtractingOnlineExecutable = " + _extractingOnlineExecutable.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    CheckingInternalExecutable = " + _checkingInternalExecutable.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    FoundInternalExecutable = " + _foundInternalExecutable.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    InternalVersion = " + (!ReferenceEquals(_internalVersion, null) ? _internalVersion.ToString(formatProvider) : @"null").Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    IsConfigurationValid = " + _isConfigurationValid.ToString(formatProvider).Replace("\n", "\n    ")) + 
                (Environment.NewLine + @"    ConfigurationErrors = " + (!ReferenceEquals(_configurationErrors, null) ? _configurationErrors.ToString(formatProvider) : @"null").Replace("\n", "\n    "))));
        }
        
        #endregion
        
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
        
        #endregion
        
        #region Property ExecutableAvailable
        
        private bool _executableAvailable;
        
        public event EventHandler ExecutableAvailableChanged;
        
        protected virtual void OnExecutableAvailableChanged()
        {
            EventHandler handler = ExecutableAvailableChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"ExecutableAvailable");
        }
        
        public virtual bool ExecutableAvailable
        {
            get { return _executableAvailable; }
            set
            {
                if ((value == _executableAvailable))
                {
                    return;
                }
                _executableAvailable = value;
                this.OnExecutableAvailableChanged();
            }
        }
        
        #endregion
        
        #region Property SelectedExecutablePath
        
        private string _selectedExecutablePath;
        
        public event EventHandler SelectedExecutablePathChanged;
        
        protected virtual void OnSelectedExecutablePathChanged()
        {
            EventHandler handler = SelectedExecutablePathChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"SelectedExecutablePath");
        }
        
        public virtual string SelectedExecutablePath
        {
            get { return _selectedExecutablePath; }
            set
            {
                if (string.Equals(value, _selectedExecutablePath))
                {
                    return;
                }
                _selectedExecutablePath = value;
                this.OnSelectedExecutablePathChanged();
            }
        }
        
        #endregion
        
        #region Property CheckingSystemExecutable
        
        private bool _checkingSystemExecutable;
        
        public event EventHandler CheckingSystemExecutableChanged;
        
        protected virtual void OnCheckingSystemExecutableChanged()
        {
            EventHandler handler = CheckingSystemExecutableChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"CheckingSystemExecutable");
        }
        
        public virtual bool CheckingSystemExecutable
        {
            get { return _checkingSystemExecutable; }
            set
            {
                if ((value == _checkingSystemExecutable))
                {
                    return;
                }
                _checkingSystemExecutable = value;
                this.OnCheckingSystemExecutableChanged();
            }
        }
        
        #endregion
        
        #region Property FoundSystemExecutable
        
        private bool _foundSystemExecutable;
        
        public event EventHandler FoundSystemExecutableChanged;
        
        protected virtual void OnFoundSystemExecutableChanged()
        {
            EventHandler handler = FoundSystemExecutableChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"FoundSystemExecutable");
        }
        
        public virtual bool FoundSystemExecutable
        {
            get { return _foundSystemExecutable; }
            set
            {
                if ((value == _foundSystemExecutable))
                {
                    return;
                }
                _foundSystemExecutable = value;
                this.OnFoundSystemExecutableChanged();
            }
        }
        
        #endregion
        
        #region Property SystemPath
        
        private string _systemPath;
        
        public event EventHandler SystemPathChanged;
        
        protected virtual void OnSystemPathChanged()
        {
            EventHandler handler = SystemPathChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"SystemPath");
        }
        
        public virtual string SystemPath
        {
            get { return _systemPath; }
            set
            {
                if (string.Equals(value, _systemPath))
                {
                    return;
                }
                _systemPath = value;
                this.OnSystemPathChanged();
            }
        }
        
        #endregion
        
        #region Property SystemVersion
        
        private string _systemVersion;
        
        public event EventHandler SystemVersionChanged;
        
        protected virtual void OnSystemVersionChanged()
        {
            EventHandler handler = SystemVersionChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"SystemVersion");
        }
        
        public virtual string SystemVersion
        {
            get { return _systemVersion; }
            set
            {
                if (string.Equals(value, _systemVersion))
                {
                    return;
                }
                _systemVersion = value;
                this.OnSystemVersionChanged();
            }
        }
        
        #endregion
        
        #region Property CheckingOnlineExecutable
        
        private bool _checkingOnlineExecutable;
        
        public event EventHandler CheckingOnlineExecutableChanged;
        
        protected virtual void OnCheckingOnlineExecutableChanged()
        {
            EventHandler handler = CheckingOnlineExecutableChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"CheckingOnlineExecutable");
        }
        
        public virtual bool CheckingOnlineExecutable
        {
            get { return _checkingOnlineExecutable; }
            set
            {
                if ((value == _checkingOnlineExecutable))
                {
                    return;
                }
                _checkingOnlineExecutable = value;
                this.OnCheckingOnlineExecutableChanged();
            }
        }
        
        #endregion
        
        #region Property FoundOnlineExecutable
        
        private bool _foundOnlineExecutable;
        
        public event EventHandler FoundOnlineExecutableChanged;
        
        protected virtual void OnFoundOnlineExecutableChanged()
        {
            EventHandler handler = FoundOnlineExecutableChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"FoundOnlineExecutable");
        }
        
        public virtual bool FoundOnlineExecutable
        {
            get { return _foundOnlineExecutable; }
            set
            {
                if ((value == _foundOnlineExecutable))
                {
                    return;
                }
                _foundOnlineExecutable = value;
                this.OnFoundOnlineExecutableChanged();
            }
        }
        
        #endregion
        
        #region Property OnlineVersion
        
        private string _onlineVersion;
        
        public event EventHandler OnlineVersionChanged;
        
        protected virtual void OnOnlineVersionChanged()
        {
            EventHandler handler = OnlineVersionChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"OnlineVersion");
        }
        
        public virtual string OnlineVersion
        {
            get { return _onlineVersion; }
            set
            {
                if (string.Equals(value, _onlineVersion))
                {
                    return;
                }
                _onlineVersion = value;
                this.OnOnlineVersionChanged();
            }
        }
        
        #endregion
        
        #region Property OnlineExecutableUrl
        
        private string _onlineExecutableUrl;
        
        public event EventHandler OnlineExecutableUrlChanged;
        
        protected virtual void OnOnlineExecutableUrlChanged()
        {
            EventHandler handler = OnlineExecutableUrlChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"OnlineExecutableUrl");
        }
        
        public virtual string OnlineExecutableUrl
        {
            get { return _onlineExecutableUrl; }
            set
            {
                if (string.Equals(value, _onlineExecutableUrl))
                {
                    return;
                }
                _onlineExecutableUrl = value;
                this.OnOnlineExecutableUrlChanged();
            }
        }
        
        #endregion
        
        #region Property DownloadingOnlineExecutable
        
        private bool _downloadingOnlineExecutable;
        
        public event EventHandler DownloadingOnlineExecutableChanged;
        
        protected virtual void OnDownloadingOnlineExecutableChanged()
        {
            EventHandler handler = DownloadingOnlineExecutableChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"DownloadingOnlineExecutable");
        }
        
        public virtual bool DownloadingOnlineExecutable
        {
            get { return _downloadingOnlineExecutable; }
            set
            {
                if ((value == _downloadingOnlineExecutable))
                {
                    return;
                }
                _downloadingOnlineExecutable = value;
                this.OnDownloadingOnlineExecutableChanged();
            }
        }
        
        #endregion
        
        #region Property FoundResourceExecutable
        
        private bool _foundResourceExecutable;
        
        public event EventHandler FoundResourceExecutableChanged;
        
        protected virtual void OnFoundResourceExecutableChanged()
        {
            EventHandler handler = FoundResourceExecutableChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"FoundResourceExecutable");
        }
        
        public virtual bool FoundResourceExecutable
        {
            get { return _foundResourceExecutable; }
            set
            {
                if ((value == _foundResourceExecutable))
                {
                    return;
                }
                _foundResourceExecutable = value;
                this.OnFoundResourceExecutableChanged();
            }
        }
        
        #endregion
        
        #region Property ExtractingOnlineExecutable
        
        private bool _extractingOnlineExecutable;
        
        public event EventHandler ExtractingOnlineExecutableChanged;
        
        protected virtual void OnExtractingOnlineExecutableChanged()
        {
            EventHandler handler = ExtractingOnlineExecutableChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"ExtractingOnlineExecutable");
        }
        
        public virtual bool ExtractingOnlineExecutable
        {
            get { return _extractingOnlineExecutable; }
            set
            {
                if ((value == _extractingOnlineExecutable))
                {
                    return;
                }
                _extractingOnlineExecutable = value;
                this.OnExtractingOnlineExecutableChanged();
            }
        }
        
        #endregion
        
        #region Property CheckingInternalExecutable
        
        private bool _checkingInternalExecutable;
        
        public event EventHandler CheckingInternalExecutableChanged;
        
        protected virtual void OnCheckingInternalExecutableChanged()
        {
            EventHandler handler = CheckingInternalExecutableChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"CheckingInternalExecutable");
        }
        
        public virtual bool CheckingInternalExecutable
        {
            get { return _checkingInternalExecutable; }
            set
            {
                if ((value == _checkingInternalExecutable))
                {
                    return;
                }
                _checkingInternalExecutable = value;
                this.OnCheckingInternalExecutableChanged();
            }
        }
        
        #endregion
        
        #region Property FoundInternalExecutable
        
        private bool _foundInternalExecutable;
        
        public event EventHandler FoundInternalExecutableChanged;
        
        protected virtual void OnFoundInternalExecutableChanged()
        {
            EventHandler handler = FoundInternalExecutableChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"FoundInternalExecutable");
        }
        
        public virtual bool FoundInternalExecutable
        {
            get { return _foundInternalExecutable; }
            set
            {
                if ((value == _foundInternalExecutable))
                {
                    return;
                }
                _foundInternalExecutable = value;
                this.OnFoundInternalExecutableChanged();
            }
        }
        
        #endregion
        
        #region Property InternalVersion
        
        private string _internalVersion;
        
        public event EventHandler InternalVersionChanged;
        
        protected virtual void OnInternalVersionChanged()
        {
            EventHandler handler = InternalVersionChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"InternalVersion");
        }
        
        public virtual string InternalVersion
        {
            get { return _internalVersion; }
            set
            {
                if (string.Equals(value, _internalVersion))
                {
                    return;
                }
                _internalVersion = value;
                this.OnInternalVersionChanged();
            }
        }
        
        #endregion
        
        #region Property IsConfigurationValid
        
        private bool _isConfigurationValid;
        
        public event EventHandler IsConfigurationValidChanged;
        
        protected virtual void OnIsConfigurationValidChanged()
        {
            EventHandler handler = IsConfigurationValidChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"IsConfigurationValid");
        }
        
        public virtual bool IsConfigurationValid
        {
            get { return _isConfigurationValid; }
            set
            {
                if ((value == _isConfigurationValid))
                {
                    return;
                }
                _isConfigurationValid = value;
                this.OnIsConfigurationValidChanged();
            }
        }
        
        #endregion
        
        #region Property ConfigurationErrors
        
        private string _configurationErrors;
        
        public event EventHandler ConfigurationErrorsChanged;
        
        protected virtual void OnConfigurationErrorsChanged()
        {
            EventHandler handler = ConfigurationErrorsChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
            this.OnPropertyChanged(@"ConfigurationErrors");
        }
        
        public virtual string ConfigurationErrors
        {
            get { return _configurationErrors; }
            set
            {
                if (string.Equals(value, _configurationErrors))
                {
                    return;
                }
                _configurationErrors = value;
                this.OnConfigurationErrorsChanged();
            }
        }
        
        #endregion
    }
    
    #endregion
}