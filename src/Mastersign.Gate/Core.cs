using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Mastersign.Gate
{
    partial class Core
    {
        private void Initialize()
        {
            SetupChanged += Core_SetupChanged;
            Setup = new Setup();
            NginxManager = new NginxManager(this);
        }

        public string ProgrammDirectory
            => System.IO.Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);

        public string AbsoluteResourceDirectory
            => System.IO.Path.IsPathRooted(ResourceDirectory)
                ? ResourceDirectory
                : System.IO.Path.Combine(ProgrammDirectory, ResourceDirectory);

        public string AbsoluteBinaryDirectory
            => System.IO.Path.IsPathRooted(BinaryDirectory)
                ? BinaryDirectory
                : System.IO.Path.Combine(ProgrammDirectory, BinaryDirectory);

        public NginxManager NginxManager { get; protected set; }

        #region Setup Observation

        private Setup ObservedSetup { get; set; }

        private void Core_SetupChanged(object sender, EventArgs e)
        {
            if (ObservedSetup != null)
                ObservedSetup.PropertyChanged -= ObservedSetup_Changed;
            ObservedSetup = Setup;
            if (ObservedSetup != null)
                ObservedSetup.PropertyChanged += ObservedSetup_Changed;
            OnWindowTitleChanged();
        }

        private void ObservedSetup_Changed(object sender, EventArgs e) => OnWindowTitleChanged();

        #endregion

        #region WindowTitle

        public string WindowTitle
        {
            get
            {
                var title = "Mastersign Gate";
                if (Setup != null)
                {
                    if (!string.IsNullOrWhiteSpace(Setup?.Name))
                    {
                        title += " - " + Setup.Name;
                    }
                    if (Setup.IsChanged)
                    {
                        title += " (changed)";
                    }
                }
                return title;
            }
        }

        public event EventHandler WindowTitleChanged;

        protected void OnWindowTitleChanged()
        {
            WindowTitleChanged?.Invoke(this, EventArgs.Empty);
            OnPropertyChanged(nameof(WindowTitle));
        }

        #endregion

        #region Project File Handling

        public bool IsProjectFileChanged => Setup != null && Setup.IsChanged;

        public void NewProjectFile()
        {
            Setup = new Setup();
            ProjectFilePath = null;
        }

        public void OpenProjectFile(string path)
        {
            Setup = new ProjectFile<Setup>(path).Load();
            Setup.Version = ProjectFile<Setup>.CURRENT_VERSION;
            Setup.AcceptChanges();
            ProjectFilePath = path;
        }

        public void SaveProjectFile()
        {
            if (Setup == null) return;
            new ProjectFile<Setup>(ProjectFilePath).Save(Setup);
            Setup.AcceptChanges();
        }

        #endregion
    }
}
