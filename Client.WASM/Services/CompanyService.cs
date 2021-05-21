using Client.WASM.Services.IServices;
using Models.DTOs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.WASM.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly HttpClient _httpClient;

        public CompanyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CompanyDTO>> GetAll()
        {
            var response = await _httpClient.GetAsync($"api/company");

            var content = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<IEnumerable<CompanyDTO>>(content);

            return obj;
        }

        public async Task<CompanyDTO> GetById(int id)
        {
            var response = await _httpClient.GetAsync($"api/company/{id}");

            var content = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<CompanyDTO>(content);

            return obj;
        }
    }
}
