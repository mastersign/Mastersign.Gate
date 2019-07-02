using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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
            ServicesChanged += ServicesChanged_Handler;
            PropertyChanged += PropertyChanged_Handler;
        }

        #region Transaction Propagation to Collection

        private ObservableCollection<Service> observedServices;

        private void ServicesChanged_Handler(object sender, EventArgs e)
        {
            if (ReferenceEquals(Services, observedServices)) return;
            if (observedServices != null)
            {
                foreach (var s in observedServices) s.PropertyChanged -= Service_Changed;
                observedServices.CollectionChanged -= Services_CollectionChanged;
            }
            observedServices = Services;
            if (observedServices != null)
            {
                foreach (var s in observedServices) s.PropertyChanged += Service_Changed;
                observedServices.CollectionChanged += Services_CollectionChanged;
            }
        }

        private void PropertyChanged_Handler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsChanged) && !IsChanged)
            {
                // Propagate commit to children
                foreach (var s in Services) s.AcceptChanges();
            }
        }

        private void Services_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (Service s in e.OldItems) s.PropertyChanged -= Service_Changed;
            }
            if (e.NewItems != null)
            {
                foreach (Service s in e.NewItems) s.PropertyChanged += Service_Changed;
            }
        }

        private void Service_Changed(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsChanged)) return;
            // Propagate change to parent
            IsChanged = true;
            OnServicesChanged();
            OnPropertyChanged(nameof(Services));
        }

        #endregion

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
