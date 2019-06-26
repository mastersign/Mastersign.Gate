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
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Mastersign.Gate
{
    /// <summary>
    /// Interaktionslogik für SetupControl.xaml
    /// </summary>
    public partial class SetupControl : UserControl
    {
        public SetupControl()
        {
            InitializeComponent();
        }

        private static string AbsolutePath(string path, params string[] possibleParents)
        {
            if (string.IsNullOrWhiteSpace(path))
                if (possibleParents.Length == 0)
                    return null;
                else
                    return AbsolutePath(possibleParents.First(), possibleParents.Skip(1).ToArray());
            if (System.IO.Path.IsPathRooted(path))
                return path;
            if (possibleParents.Length == 0)
                return System.IO.Path.Combine(
                    Environment.CurrentDirectory,
                    path);
            return System.IO.Path.Combine(
                AbsolutePath(possibleParents.First(), possibleParents.Skip(1).ToArray())
                    ?? Environment.CurrentDirectory,
                path);
        }

        private string FindFolder(string name, params string[] initialPathCandidates)
        {
            var currentPath = initialPathCandidates.FirstOrDefault(
                p => !string.IsNullOrWhiteSpace(p) && System.IO.Directory.Exists(p))
                ?? Environment.CurrentDirectory;
            var dlg = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "Choose " + name + "...",
                InitialDirectory = currentPath,
            };
            return dlg.ShowDialog(Window.GetWindow(this)) == CommonFileDialogResult.Ok
                ? dlg.FileName : null;
        }

        private void BtnDirectoryBrowse_Click(object sender, RoutedEventArgs e)
        {
            txtDirectory.Text = FindFolder("Configuration Directory",
                AbsolutePath(txtDirectory.Text))
                ?? txtDirectory.Text;
        }

        private void BtnLogDirectoryBrowse_Click(object sender, RoutedEventArgs e)
        {
            txtLogDirectory.Text = FindFolder("Log Directory",
                AbsolutePath(txtLogDirectory.Text, txtDirectory.Text),
                AbsolutePath(txtDirectory.Text))
                ?? txtLogDirectory.Text;
        }

        private void BtnCertificateDirectoryBrowse_Click(object sender, RoutedEventArgs e)
        {
            txtCertificateDirectory.Text = FindFolder("Certificate Directory",
                AbsolutePath(txtCertificateDirectory.Text, txtDirectory.Text),
                AbsolutePath(txtDirectory.Text))
                ?? txtCertificateDirectory.Text;
        }

        private void ServiceNew_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ServiceNew_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!(DataContext is Setup setup) || setup.Server == null) return;
            setup.Server.Services.Add(new Service());
        }

        private void ServiceMoveUp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!(DataContext is Setup setup) || setup.Server == null) return;
            e.CanExecute = setup.Server.Services.IndexOf(e.Parameter as Service) > 0;
        }

        private void ServiceMoveUp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var setup = (Setup)DataContext;
            var p = setup.Server.Services.IndexOf((Service)e.Parameter);
            setup.Server.Services.Move(p, p - 1);
        }

        private void ServiceMoveDown_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!(DataContext is Setup setup) || setup.Server == null) return;
            e.CanExecute = setup.Server.Services.IndexOf(e.Parameter as Service) < setup.Server.Services.Count - 1;
        }

        private void ServiceMoveDown_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var setup = (Setup)DataContext;
            var p = setup.Server.Services.IndexOf((Service)e.Parameter);
            setup.Server.Services.Move(p, p + 1);
        }

        private void ServiceDelete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!(DataContext is Setup setup) || setup.Server == null) return;
            e.CanExecute = true;
        }

        private void ServiceDelete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var setup = (Setup)DataContext;
            setup.Server.Services.Remove((Service)e.Parameter);
        }

    }
}
