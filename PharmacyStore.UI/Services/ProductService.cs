using Blazored.LocalStorage;
using PharmacyStore.UI.Models.ViewModels;
using PharmacyStore.UI.Services.IServices;
using System.Net.Http;

namespace PharmacyStore.UI.Services
{
    public class ProductService : BaseService<ProductViewModel>, IProductService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public ProductService(IHttpClientFactory httpClient, ILocalStorageService localStorageService) : base(httpClient, localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }
    }
}
