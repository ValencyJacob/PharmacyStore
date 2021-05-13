using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using PharmacyStore.UI.Models;
using PharmacyStore.UI.Models.ViewModels;
using PharmacyStore.UI.Providers;
using PharmacyStore.UI.Services.IServices;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyStore.UI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authenticationState;

        public AuthenticationService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService,
            AuthenticationStateProvider authenticationState)
        {
            _httpClientFactory = httpClientFactory;
            _localStorageService = localStorageService;
            _authenticationState = authenticationState;
        }

        public async Task<bool> Login(LoginViewModel model)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, EndpointsService.LoginEndpoint);

            request.Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var content = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<TokenResponse>(content);

            await _localStorageService.SetItemAsync("authToken", token.Token);

            await ((APIAuthStateProvider)_authenticationState).LoggedIn();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Token);

            return true;
        }

        public async Task Logout()
        {
            await _localStorageService.RemoveItemAsync("authToken");
            ((APIAuthStateProvider)_authenticationState).LoggedOut();
        }

        public async Task<bool> Registration(RegistrationViewModel model)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, EndpointsService.RegistrationEndpoint);

            request.Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
    }
}
