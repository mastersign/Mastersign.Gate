using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaktionslogik für DependencyControl.xaml
    /// </summary>
    public partial class DependencyControl : UserControl
    {
        public DependencyControl()
        {
            InitializeComponent();
        }

        public void NginxDownload_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!(DataContext is NginxManager nginx)) return;
            e.CanExecute = nginx.MonitorState.FoundOnlineExecutable;
        }

        public async void NginxDownload_Executed(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is NginxManager nginx)) return;
            await nginx.DownloadOnlineExecutable();
            if (nginx.MonitorState.FoundResourceExecutable)
            {
                await nginx.ExtractOnlineExecutable();
                await nginx.FindInternalExecutable();
            }
        }

        private void OnlineZipFile_Click(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is NginxManager nginx)) return;
            Process.Start(nginx.MonitorState.OnlineExecutableUrl);
        }
    }
}
