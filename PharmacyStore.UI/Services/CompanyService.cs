using Blazored.LocalStorage;
using PharmacyStore.UI.Models.ViewModels;
using PharmacyStore.UI.Services.IServices;
using System.Net.Http;

namespace PharmacyStore.UI.Services
{
    public class CompanyService : BaseService<CompanyViewModel>, ICompanyService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public CompanyService(IHttpClientFactory httpClient, ILocalStorageService localStorageService) : base(httpClient, localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }
    }
}
