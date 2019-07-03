using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mastersign.Gate
{
    /// <summary>
    /// Interaktionslogik für RunControl.xaml
    /// </summary>
    public partial class RunControl : UserControl
    {
        public RunControl()
        {
            InitializeComponent();
        }

        private NginxManager NginxManager => (NginxManager)DataContext;

        public void HttpFrontendUrl_Click(object sender, RoutedEventArgs e)
            => System.Diagnostics.Process.Start(NginxManager.State.HttpFrontendUrl);

        public void HttpsFrontendUrl_Click(object sender, RoutedEventArgs e)
            => System.Diagnostics.Process.Start(NginxManager.State.HttpsFrontendUrl);

        private void CopyHttpFrontendUrl_Handler(object sender, RoutedEventArgs e)
            => Clipboard.SetText(NginxManager.State.HttpFrontendUrl);

        private void CopyHttpsFrontendUrl_Handler(object sender, RoutedEventArgs e)
            => Clipboard.SetText(NginxManager.State.HttpsFrontendUrl);

        private async void SetupConfigDirectory_Handler(object sender, RoutedEventArgs e)
        { 
            await NginxManager.SetupConfigDirectory();
            await NginxManager.CheckConfigDirectory();
            if (NginxManager.State.IsServerRunning)
            {
                await NginxManager.ReloadServerConfig();
            }
        }
    }
}
