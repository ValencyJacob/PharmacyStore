using System.Collections.Generic;
using System.Threading.Tasks;

namespace PharmacyStore.UI.Services.IServices
{
    public interface IBaseService<T> where T : class
    {
        Task<T> GetByIdAsync(string url, int id);
        Task<IList<T>> GetAllAsync(string url);
        Task<bool> CreateAsync(string url, T obj);
        Task<bool> UpdateAsync(string url, T obj, int id);
        Task<bool> DeleteAsync(string url, int id);
    }
}
