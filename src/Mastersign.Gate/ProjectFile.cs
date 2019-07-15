using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.TypeInspectors;

namespace Mastersign.Gate
{
    public class ProjectFile<T>
    {
        private const string LEADING_COMMENT = "Mastersign Gate Project File";
        public const string CURRENT_VERSION = "1.1";
        private static readonly string[] SUPPORTED_VERSIONS = new[] { "1", "1.0", "1.1" };
        private const int PROJECT_LOAD_RETRY_TIMEOUT_MS = 2000;
        private const int PROJECT_LOAD_RETRY_INTERVAL_MS = 100;

        private IDeserializer deserializer;
        private ISerializer serializer;

        public string FilePath { get; }

        public ProjectFile(string filePath)
        {
            InitializeSerialization();
            FilePath = filePath;
        }

        private void InitializeSerialization()
        {
            deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();
            serializer = new SerializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .WithTypeInspector(innerInspector => new SuppressChangeTrackingPropertiesTypeInspector(innerInspector))
                .WithTypeInspector(innerInspector => new LeadingVersionTypeInspector(innerInspector))
                .Build();
        }

        private class LeadingVersionTypeInspector : TypeInspectorSkeleton
        {
            private readonly ITypeInspector innerTypeDescriptor;

            public LeadingVersionTypeInspector(ITypeInspector innerTypeDescriptor)
            {
                this.innerTypeDescriptor = innerTypeDescriptor;
            }

            public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object container)
            {
                var properties = innerTypeDescriptor.GetProperties(type, container).ToList();
                var versionProperty = properties.FirstOrDefault(
                    p => string.Equals(p.Name, "Version", StringComparison.InvariantCultureIgnoreCase));
                if (versionProperty != null) yield return versionProperty;
                foreach (var p in properties)
                {
                    if (p == versionProperty) continue;
                    yield return p;
                }
            }
        }

        private class SuppressChangeTrackingPropertiesTypeInspector : TypeInspectorSkeleton
        {
            private readonly ITypeInspector innerTypeDescriptor;

            public SuppressChangeTrackingPropertiesTypeInspector(ITypeInspector innerTypeDescriptor)
            {
                this.innerTypeDescriptor = innerTypeDescriptor;
            }

            public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object container)
            {
                return innerTypeDescriptor.GetProperties(type, container).Where(
                    p => !string.Equals(p.Name, "IsChanged", StringComparison.InvariantCultureIgnoreCase));
            }
        }

        private static readonly Regex VersionPattern = new Regex(@"^version\:\s+(['""]?)(?<version>\d+(?:\.\d+)?(?:-[a-z-\d])?)\1\s*$");

        private static string FindVersionString(TextReader r)
        {
            // TODO make compatible with JSON by leveraging a YAML and JSON compatible streaming API
            var line = r.ReadLine();
            while (line != null)
            {
                var m = VersionPattern.Match(line);
                if (m.Success) return m.Groups["version"].Value;
                line = r.ReadLine();
            }
            return null;
        }

        private static void CheckVersionSupport(Stream s, out string version)
        {
            using (var r = new StreamReader(s, Encoding.UTF8, false, 1024, true))
            {
                version = FindVersionString(r);
            }
            if (version == null) throw new FormatException("No version attribute found.");
            if (!SUPPORTED_VERSIONS.Contains(version)) throw new FormatException("Version not supported.");
            s.Seek(0, SeekOrigin.Begin);
        }

        public T Load()
        {
            Stream s = null;

            var w = new Stopwatch();
            w.Start();
            Exception exc = null;
            // Retry opening the file multiple times in case the file is currently updated by another process
            while (s == null && w.ElapsedMilliseconds < PROJECT_LOAD_RETRY_TIMEOUT_MS)
            {
                try
                {
                    s = File.Open(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    exc = null;
                }
                catch (IOException ioe)
                {
                    exc = ioe;
                    Thread.Sleep(PROJECT_LOAD_RETRY_INTERVAL_MS);
                }
            }
            w.Stop();
            if (exc != null) throw new ProjectLoadingFailedException(
                exc.RecursiveMessage(), FilePath); ;

            string version = null;
            try
            {
                // Check for a line with matching version string
                CheckVersionSupport(s, out version);

                // Try to deserialize as a YAML document
                using (var r = new StreamReader(s, Encoding.UTF8))
                {
                    return deserializer.Deserialize<T>(r);
                }
            }
            catch (Exception exc2)
            {
                // throw new custom exception with merged error message
                throw new ProjectLoadingFailedException(
                    exc2.RecursiveMessage(), FilePath, version);
            }
        }

        public void Save(T project)
        {
            var utf8WithoutBOM = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
            try
            {
                using (var w = new StreamWriter(FilePath, append: false, encoding: utf8WithoutBOM))
                {
                    w.WriteLine($"# {LEADING_COMMENT}");
                    serializer.Serialize(w, project);
                }
            }
            catch (Exception exc)
            {
                throw new ProjectSavingFailedException(exc.RecursiveMessage(), FilePath);
            }
        }
    }

    public class ProjectLoadingFailedException : Exception
    {
        public string FilePath { get; }

        public string FileVersion { get; }

        public ProjectLoadingFailedException(string message, string filePath, string fileVersion = null)
            : base(message)
        {
            FilePath = filePath;
            FileVersion = fileVersion;
        }
    }

    public class ProjectSavingFailedException : Exception
    {
        public string FilePath { get; }

        public ProjectSavingFailedException(string message, string filePath)
            : base(message)
        {
            FilePath = filePath;
        }
    }
}
