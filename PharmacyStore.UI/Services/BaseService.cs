using Blazored.LocalStorage;
using Newtonsoft.Json;
using PharmacyStore.UI.Services.IServices;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyStore.UI.Services
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public BaseService(IHttpClientFactory httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<bool> CreateAsync(string url, T obj)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if(obj == null)
            {
                return false;
            }

            request.Content = new StringContent(JsonConvert.SerializeObject(obj));

            var client = _httpClient.CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetBearerToken());

            HttpResponseMessage response = await client.SendAsync(request);

            if(response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAsync(string url, int id)
        {
            if(id == 0)
            {
                return false;
            }

            var request = new HttpRequestMessage(HttpMethod.Delete, url + id);

            var client = _httpClient.CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetBearerToken());

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }

            return false;
        }

        public async Task<IList<T>> GetAllAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var client = _httpClient.CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetBearerToken());

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IList<T>>(content);
            }

            return null;
        }

        public async Task<T> GetByIdAsync(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + id);

            var client = _httpClient.CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetBearerToken());

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }

            return null;
        }

        public async Task<bool> UpdateAsync(string url, T obj, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url + id);

            if (obj == null)
                return false;

            request.Content = new StringContent(JsonConvert.SerializeObject(obj)
                , Encoding.UTF8, "application/json");

            var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", await GetBearerToken());

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;

            return false;
        }

        private async Task<string> GetBearerToken()
        {
            return await _localStorageService.GetItemAsync<string>("authToken");
        }
    }
}
