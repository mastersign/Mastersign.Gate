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
    public partial class SetupServicesControl : UserControl
    {
        public SetupServicesControl()
        {
            InitializeComponent();
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
