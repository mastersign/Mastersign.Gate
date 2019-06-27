using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mastersign.Gate
{
    partial class Core
    {
        private void Initialize()
        {
            SetupChanged += Core_SetupChanged;
            Setup = new Setup();
        }

        #region Setup Observation

        private Setup ObservedSetup { get; set; }

        private void Core_SetupChanged(object sender, EventArgs e)
        {
            if (ObservedSetup != null)
                ObservedSetup.NameChanged -= ObservedSetup_NameChanged;
            ObservedSetup = Setup;
            if (ObservedSetup != null)
                ObservedSetup.NameChanged += ObservedSetup_NameChanged;
            OnWindowTitleChanged();
        }

        private void ObservedSetup_NameChanged(object sender, EventArgs e) => OnWindowTitleChanged();

        #endregion

        #region WindowTitle

        public string WindowTitle => string.IsNullOrWhiteSpace(Setup?.Name)
            ? "Mastersign Gate"
            : "Mastersign Gate - " + Setup.Name;

        public event EventHandler WindowTitleChanged;

        protected void OnWindowTitleChanged()
        {
            WindowTitleChanged?.Invoke(this, EventArgs.Empty);
            OnPropertyChanged(nameof(WindowTitle));
        }

        #endregion
    }
}
