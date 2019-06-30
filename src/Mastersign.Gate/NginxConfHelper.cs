using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastersign.Gate
{
    internal static class NginxConfHelper
    {
        private const string SPC = " ";
        private const string DELIM = ";";
        private const string INDENT = "    ";

        public static IEnumerable<string> NoLines()
        {
            yield break;
        }

        public static IEnumerable<string> Chain(IEnumerable<IEnumerable<string>> blocks)
        {
            foreach (var block in blocks)
            {
                foreach (var line in block)
                {
                    yield return line;
                }
            }
        }

        public static IEnumerable<string> Chain(params IEnumerable<string>[] blocks)
        {
            return Chain((IEnumerable<IEnumerable<string>>)blocks);
        }

        public static IEnumerable<string> Setting(string name, params string[] values)
        {
            yield return name + SPC + string.Join(SPC, values) + DELIM;
        }

        public static IEnumerable<string> Indent(IEnumerable<string> lines)
        {
            return lines.Select(line => INDENT + line);
        }

        public static IEnumerable<string> Block(string name, IEnumerable<string> content, params string[] values)
        {
            yield return name + SPC
                + (values.Length > 0 ? SPC + string.Join(SPC, values) : string.Empty)
                + SPC + "{";
            foreach (var line in Indent(content)) yield return line;
            yield return "}";
        }

        public static string FsPath(string path)
            => path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar)
                   .Replace(Path.DirectorySeparatorChar, '/');

        public static string FsPath(params string[] parts)
            => FsPath(Path.Combine(parts));
    }
}
