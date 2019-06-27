using MahApps.Metro.Controls;
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
        }

        public void ProjectFileOpen_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public void ProjectFileOpen_Executed(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void ProjectFileSave_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public void ProjectFileSave_Executed(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
