using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BRPLUSA.Core.Utilities
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
            return string.IsNullOrEmpty(path)
                ? null
                : Directory.EnumerateFiles(
                        Path.GetDirectoryName(path),
                        extension,
                        SearchOption.AllDirectories)
                    .ToArray();
        }

        public static string ParseForBrplusaProjectNumber(string filePath)
        {
            var project = "";

            var chars = filePath.ToCharArray();

            for (var i = 0; i < filePath.Length; i++)
            {
                var c = chars[i];
                if (project.Length == 10 && StringProcessor.ContainsNumbers(project))
                    break;

                if (!StringProcessor.IsAlphanumeric(c) && !c.Equals('.'))
                {
                    project = "";
                    continue;
                }

                project += c;
            }

            return project;
        }
    }

    public static class StringProcessor
    {
        public static bool IsNumber(char c)
        {
            return int.TryParse(c.ToString(), out var n);
        }

        public static bool IsAlphabetic(char c)
        {
            var regex = new Regex("^[a-zA-Z]*$");

            return regex.IsMatch(c.ToString());
        }

        public static bool IsPunctuation(char c)
        {
            return !IsNumber(c) && !IsAlphabetic(c);
        }

        public static bool IsAlphanumeric(char c)
        {
            var regex = new Regex("^[a-zA-Z0-9]*$");

            return regex.IsMatch(c.ToString());
        }

        public static bool ContainsNumbers(string project)
        {
            return project.Any(IsNumber);
        }
    }
}
