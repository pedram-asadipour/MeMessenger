using System;
using System.IO;
using _Framework.FileManager;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ServerHost.Framework.FileManager
{
    public class FileManager : IFileManager
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileManager(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string Uploader(IFormFile file, string path)
        {
            if (file == null) return null;

            var directory = $@"{_webHostEnvironment.WebRootPath}\Uploads\{path}";

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var random = new Random();

            var fileName = $"{random.Next()}{random.Next()}{Path.GetExtension(file.FileName)}";

            var directoryFile = $@"{directory}\{fileName}";

            using var output = File.Create(directoryFile);
            file.CopyTo(output);

            return $"{path}/{fileName}";
        }

        public void Remove(string path)
        {
            path = $@"{_webHostEnvironment.WebRootPath}\Uploads\{path}";

            if (string.IsNullOrWhiteSpace(path))
                return;

            if(!File.Exists(path))
                return;
            
            File.Delete(path);
        }
    }
}