using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRPLUSA.Core
{
    public static class FileUtils
    {
        public static void CreateDirectory(string path)
        {
            if (Directory.Exists(path))
                return;

            Directory.CreateDirectory(path);
        }

        public static IEnumerable<string> GetAllFilesOfType(string path, string extension)
        {
            return String.IsNullOrEmpty(path)
                ? null
                : Directory.EnumerateFiles(
                        Path.GetDirectoryName(path),
                        extension,
                        SearchOption.AllDirectories)
                    .ToArray();
        }
    }
}
