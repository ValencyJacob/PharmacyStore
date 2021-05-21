using Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.WASM.Services.IServices
{
    public interface ICompanyService
    {
        //Get all amenities.
        public Task<IEnumerable<CompanyDTO>> GetAll();

        //Get individual flat.
        public Task<CompanyDTO> GetById(int id);
    }
}
