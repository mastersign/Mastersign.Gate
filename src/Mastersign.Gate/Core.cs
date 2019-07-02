using System;
using System.Collections.Generic;
using System.IO;
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
        private static readonly string[] DEFAULT_PROJECT_FILES = new[] { "mgate.yml", "mgate.yaml" };

        private void Initialize()
        {
            SetupChanged += Core_SetupChanged;
            Setup = new Setup { Core = this };
            NginxManager = new NginxManager(this);
        }

        public void Start(string[] cliArgs)
        {
            if (cliArgs.Length == 0)
            {
                var defaultProject = DEFAULT_PROJECT_FILES
                    .Select(fileName => Path.Combine(Environment.CurrentDirectory, fileName))
                    .FirstOrDefault(path => File.Exists(path));
                if (defaultProject != null) OpenProjectFile(defaultProject);
            }
            else if (cliArgs.Length == 1 && File.Exists(cliArgs[0]))
            {
                OpenProjectFile(cliArgs[0]);
            }
        }

        public string ProgrammDirectory
            => Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);

        public string AbsoluteResourceDirectory
            => Path.IsPathRooted(ResourceDirectory)
                ? ResourceDirectory
                : Path.Combine(ProgrammDirectory, ResourceDirectory);

        public string AbsoluteBinaryDirectory
            => Path.IsPathRooted(BinaryDirectory)
                ? BinaryDirectory
                : Path.Combine(ProgrammDirectory, BinaryDirectory);

        public string AbsoluteProjectDirectory
            => string.IsNullOrWhiteSpace(ProjectFilePath)
                ? Environment.CurrentDirectory
                : Path.GetDirectoryName(ProjectFilePath);

        public string AbsoluteConfigDirectory
            => string.IsNullOrWhiteSpace(Setup.Directory)
                ? AbsoluteProjectDirectory
                : Path.IsPathRooted(Setup.Directory)
                    ? Setup.Directory
                    : Path.Combine(AbsoluteProjectDirectory, Setup.Directory);

        public string AbsoluteConfigPath(string path)
            => string.IsNullOrWhiteSpace(path)
                ? AbsoluteConfigDirectory
                : Path.IsPathRooted(path)
                    ? path
                    : Path.Combine(AbsoluteConfigDirectory, path);

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
