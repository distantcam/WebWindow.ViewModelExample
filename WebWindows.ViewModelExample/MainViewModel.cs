using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace WebWindows.ViewModelExample
{
    public class MainViewModel : ViewModel
    {
        public string Title { get; set; }
        public object DirectoryInfo { get; set; }
        public object FileInfo { get; set; }

        public void NavigateTo(JsonElement parsedMessage)
        {
            var basePath = parsedMessage.GetProperty("basePath").GetString();
            var relativePath = parsedMessage.GetProperty("relativePath").GetString();
            var destinationPath = Path.GetFullPath(Path.Combine(basePath, relativePath));
            ShowDirectoryInfo(destinationPath);
        }

        public void ShowFile(JsonElement parsedMessage)
        {
            var fullName = parsedMessage.GetProperty("fullName").GetString();
            var fileInfo = new FileInfo(fullName);
            FileInfo = null; // Clear the old display first
            FileInfo = new
            {
                name = fileInfo.Name,
                size = fileInfo.Length,
                fullName = fileInfo.FullName,
                text = ReadTextFile(fullName, maxChars: 100000),
            };
        }

        protected override void OnViewReady() => ShowDirectoryInfo(Directory.GetCurrentDirectory());

        void ShowDirectoryInfo(string path)
        {
            Title = path;

            var directoryInfo = new DirectoryInfo(path);
            DirectoryInfo = new
            {
                name = path,
                isRoot = Path.GetDirectoryName(path) == null,
                directories = directoryInfo.GetDirectories().Select(directoryInfo => new
                {
                    name = directoryInfo.Name + Path.DirectorySeparatorChar,
                }),
                files = directoryInfo.GetFiles().Select(fileInfo => new
                {
                    name = fileInfo.Name,
                    size = fileInfo.Length,
                    fullName = fileInfo.FullName,
                }),
            };
        }

        static string ReadTextFile(string fullName, int maxChars)
        {
            var stringBuilder = new StringBuilder();
            var buffer = new char[4096];
            using var file = File.OpenText(fullName);
            int charsRead = int.MaxValue;
            while (maxChars > 0 && charsRead > 0)
            {
                charsRead = file.ReadBlock(buffer, 0, Math.Min(maxChars, buffer.Length));
                stringBuilder.Append(buffer, 0, charsRead);
                maxChars -= charsRead;
            }
            return stringBuilder.ToString();
        }
    }
}
