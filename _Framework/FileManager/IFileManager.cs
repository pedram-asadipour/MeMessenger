using Microsoft.AspNetCore.Http;

namespace _Framework.FileManager
{
    public interface IFileManager
    {
        string Uploader(IFormFile file, string path);
        void Remove(string path);
    }
}