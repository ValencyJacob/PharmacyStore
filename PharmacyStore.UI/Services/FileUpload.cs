using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PharmacyStore.UI.Services.IServices;
using System.IO;
using System.Threading.Tasks;

namespace PharmacyStore.UI.Services
{
    public class FileUpload : IFileUpload
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileUpload(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public void RemoveFile(string pictureName)
        {
            var path = $"{_env.WebRootPath}\\images\\{pictureName}";

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public async Task<string> UploadFile(Stream msFile, string pictureName)
        {
            var path = $"{_env.WebRootPath}\\images\\{pictureName}";

            var buffer = new byte[4 * 1096];

            int bytesRead;

            double totalRead = 0;

            using FileStream fs = new FileStream(path, FileMode.Create);

            while ((bytesRead = await msFile.ReadAsync(buffer)) != 0)
            {
                totalRead += bytesRead;
                await fs.WriteAsync(buffer);
            }

            var url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}/";
            
            var fullPath = $"{url}images/{pictureName}";

            return fullPath;
        }
    }
}
