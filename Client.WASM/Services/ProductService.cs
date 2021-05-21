using Client.WASM.Services.IServices;
using Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.WASM.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ProductDTO>> GetAll()
        {
            var response = await _httpClient.GetAsync($"api/product");

            var content = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(content);

            return obj;
        }

        public async Task<ProductDTO> GetById(int id)
        {
            var response = await _httpClient.GetAsync($"api/company/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var obj = JsonConvert.DeserializeObject<ProductDTO>(content);

                return obj;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();

                var error = JsonConvert.DeserializeObject<ErrorDTO>(content);

                Console.WriteLine(error.ErrorMessage);

                throw new Exception(error.ErrorMessage);
            }
        }
    }
}
