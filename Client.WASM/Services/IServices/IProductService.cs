using Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.WASM.Services.IServices
{
    public interface IProductService
    {
        //Get all amenities.
        public Task<IEnumerable<ProductDTO>> GetAll();

        //Get individual flat.
        public Task<ProductDTO> GetById(int id);
    }
}
