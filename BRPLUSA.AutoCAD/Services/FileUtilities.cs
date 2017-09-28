using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRPLUSA.AutoCAD.Services
{
    public static class FileUtilities
    {
        /// <summary>
        /// Recursively gathers all drawing files
        /// 
        /// Drawing files are any with an .dwg file extension
        /// </summary>
        /// <param name="path">file to start search from</param>
        public static IEnumerable<string> GetAllDrawingFiles(string path)
        {
            return GetAllFilesOfType(path, "*.dwg");
        }

        public static IEnumerable<string> GetAllFilesOfType(string path, string extension)
        {
            return string.IsNullOrEmpty(path) 
                ? null 
                : Directory.EnumerateFiles(
                    Path.GetDirectoryName(path), 
                    extension, 
                    SearchOption.AllDirectories)
                .ToArray();
        }
    }
}
