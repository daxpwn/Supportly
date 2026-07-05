using Application;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace Supportly.API.Storage
{
    // Skladištenje fajlova na disk: wwwroot/Uploads. Ime na disku je GUID (bez path traversal-a),
    // originalno ime se čuva u bazi (Attachment.FileName).
    public class DiskFileStorage : IFileStorage
    {
        private const string SubFolder = "Uploads";
        private readonly string _webRoot;

        public DiskFileStorage(IWebHostEnvironment env)
        {
            _webRoot = env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot");
        }

        public string Save(byte[] content, string originalFileName)
        {
            var ext = Path.GetExtension(originalFileName);
            var storedName = Guid.NewGuid().ToString("N") + ext;

            var folder = Path.Combine(_webRoot, SubFolder);
            Directory.CreateDirectory(folder);

            File.WriteAllBytes(Path.Combine(folder, storedName), content);

            // relativna putanja koja je ujedno URL (uz app.UseStaticFiles())
            return $"/{SubFolder}/{storedName}";
        }

        public void Delete(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return;

            var full = Path.Combine(_webRoot,
                relativePath.TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(full))
                File.Delete(full);
        }
    }
}
