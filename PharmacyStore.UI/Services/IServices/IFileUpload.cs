using System.IO;
using System.Threading.Tasks;

namespace PharmacyStore.UI.Services.IServices
{
    public interface IFileUpload
    {
        public Task<string> UploadFile(Stream msFile, string picName);
        public void RemoveFile(string picName);
    }
}
