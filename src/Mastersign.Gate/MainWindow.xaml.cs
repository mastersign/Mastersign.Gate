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
            Startup();
        }

        private async void Startup()
        {
            await Core.NginxManager.UpdateState();
            if (Core.RunAtStart)
            {
                await Dispatcher.BeginInvoke((Action)(() => tabs.SelectedItem = tabRun));
                Core.NginxManager.State.IsServerRunning = true;
            }
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
            dlg.Filters.Add(new CommonFileDialogFilter("YAML File", "*.yml;*.yaml"));
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
                dlg.Filters.Add(new CommonFileDialogFilter("YAML File", "*.yml;*.yaml"));
                dlg.DefaultFileName = "mgate.yml";
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
    }
}
