using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Mastersign.Gate
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        public Core Core { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ShutdownMode = ShutdownMode.OnMainWindowClose;

            ThemeManager.IsAutomaticWindowsAppModeSettingSyncEnabled = true;

            Core = new Core();
            Core.Start(e.Args);
        }
    }
}
