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

        public void NginxFindDownload_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!(DataContext is Core core)) return;
            e.CanExecute = true;
        }

        public async void NginxFindDownload_Executed(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is Core core)) return;
            await core.NginxManager.FindOnlineExecutable();
            CommandManager.InvalidateRequerySuggested();
        }

        public void NginxDownload_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!(DataContext is Core core)) return;
            e.CanExecute = core.NginxManager.State.FoundOnlineExecutable;
        }

        public async void NginxDownload_Executed(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is Core core)) return;
            try
            {
                await core.NginxManager.DownloadOnlineExecutable();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(
                    "Writing to the resource directory was denied." + Environment.NewLine +
                    Environment.NewLine +
                    "Resource directory: " + core.AbsoluteResourceDirectory + Environment.NewLine +
                    Environment.NewLine +
                    "You may have to start Mastersign.Gate with Administrator priviledges to update Nginx.",
                    "Downloading Nginx...",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (System.Net.WebException wex)
            {
                MessageBox.Show(
                    "Error during download of Nginx:" + Environment.NewLine +
                    Environment.NewLine +
                    wex.Message + Environment.NewLine +
                    wex.InnerException?.Message + Environment.NewLine +
                    "(" + wex.InnerException?.GetType().FullName + ")",
                    "Downloading Nginx...",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error during download of Nginx:" + Environment.NewLine +
                    Environment.NewLine +
                    ex.Message + Environment.NewLine +
                    "(" + ex.GetType().FullName + ")",
                    "Downloading Nginx...",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (core.NginxManager.State.FoundResourceExecutable)
            {
                try
                {
                    await core.NginxManager.ExtractOnlineExecutable();
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show(
                        "Writing to the Nginx directory was denied." + Environment.NewLine +
                        Environment.NewLine +
                        "Nginx directory: " + core.AbsoluteBinaryDirectory + Environment.NewLine +
                        Environment.NewLine +
                        "You may have to start Mastersign.Gate with Administrator priviledges to update Nginx.",
                        "Extracting Nginx...",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Error during extraction of Nginx:" + Environment.NewLine +
                        Environment.NewLine +
                        ex.Message + Environment.NewLine +
                        "(" + ex.GetType().FullName + ")",
                        "Extracting Nginx...",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                await core.NginxManager.FindInternalExecutable();
            }
        }

        private void OnlineZipFile_Click(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is Core core)) return;
            Process.Start(core.NginxManager.State.OnlineExecutableUrl);
        }
    }
}
