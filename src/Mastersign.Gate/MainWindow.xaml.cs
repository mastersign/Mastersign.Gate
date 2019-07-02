using MahApps.Metro.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;
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
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private Core Core => ((App)Application.Current).Core;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = Core;
            _ = Core.NginxManager.UpdateState();
        }

        public void ProjectFileNew_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public void ProjectFileNew_Executed(object sender, RoutedEventArgs e)
        {
            Core.NewProjectFile();
        }

        public void ProjectFileOpen_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public void ProjectFileOpen_Executed(object sender, RoutedEventArgs e)
        {
            var dlg = new CommonOpenFileDialog()
            {
                Title = "Open Mastersign Gate Project...",
            };
            dlg.Filters.Add(new CommonFileDialogFilter("YAML File", "*.yaml;*.yml"));
            if (dlg.ShowDialog(this) != CommonFileDialogResult.Ok) return;

            try
            {
                Core.OpenProjectFile(dlg.FileName);
            }
            catch (ProjectLoadingFailedException exc)
            {
                MessageBox.Show(exc.Message,
                    "Loading Project File" + (exc.FileVersion != null ? " v" + exc.FileVersion : ""),
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ProjectFileSave_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public void ProjectFileSave_Executed(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Core.ProjectFilePath))
            {
                var dlg = new CommonSaveFileDialog()
                {
                    Title = "Save Mastersign Gate Project...",
                };
                dlg.Filters.Add(new CommonFileDialogFilter("YAML File (*.yaml, *.yml)", "*.yaml;*.yml"));
                dlg.DefaultExtension = ".yml";
                if (dlg.ShowDialog(this) != CommonFileDialogResult.Ok) return;
                Core.ProjectFilePath = dlg.FileName;
            }

            try
            {
                Core.SaveProjectFile();
            }
            catch (ProjectSavingFailedException exc)
            {
                MessageBox.Show(exc.Message,
                    "Saving Project File",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //private async void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    await Task.WhenAll(
        //        Core.NginxManager.FindSystemExecutable(),
        //        Core.NginxManager.FindInternalExecutable(),
        //        Core.NginxManager.FindOnlineExecutable());
        //    if (Core.NginxManager.State.FoundOnlineExecutable)
        //    {
        //        await Core.NginxManager.DownloadOnlineExecutable();
        //        if (Core.NginxManager.State.FoundResourceExecutable)
        //        {
        //            await Core.NginxManager.ExtractOnlineExecutable();
        //            await Core.NginxManager.FindInternalExecutable();
        //        }
        //    }
        //}

        //private async void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    string AbsolutePath(string root, string path)
        //        => string.IsNullOrWhiteSpace(path)
        //            ? root
        //            : System.IO.Path.IsPathRooted(path) ? path : System.IO.Path.Combine(root, path);

        //    var configDir = AbsolutePath(Environment.CurrentDirectory, Core.Setup.Directory);
        //    var logDir = AbsolutePath(configDir, Core.Setup.LogDirectory);
        //    var certDir = AbsolutePath(configDir, Core.Setup.CertificateDirectory);
        //    await Core.NginxManager.SetupConfigDirectory(configDir, logDir, certDir, Core.Setup);
        //    await Core.NginxManager.CheckConfigDirectory();
        //}
    }
}
