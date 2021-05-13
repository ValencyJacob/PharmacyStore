using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Linq;

namespace PharmacyStore.UI.Providers
{
    public class APIAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public APIAuthStateProvider(ILocalStorageService localStorageService, JwtSecurityTokenHandler tokenHandler)
        {
            _localStorageService = localStorageService;
            _tokenHandler = tokenHandler;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var savedToken = await _localStorageService.GetItemAsync<string>("authToken");

                if (string.IsNullOrWhiteSpace(savedToken))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var tokenContent = _tokenHandler.ReadJwtToken(savedToken);

                var expiry = tokenContent.ValidTo;

                if (expiry < DateTime.Now)
                {
                    await _localStorageService.RemoveItemAsync("authToken");
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var claims = ParseClaims(tokenContent);

                var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

                return new AuthenticationState(user);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public async Task LoggedIn()
        {
            var savedToken = await _localStorageService.GetItemAsync<string>("authToken");
            var tokenContent = _tokenHandler.ReadJwtToken(savedToken);
            var claims = ParseClaims(tokenContent);
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }

        public void LoggedOut()
        {
            var nobody = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(nobody));
            NotifyAuthenticationStateChanged(authState);
        }

        public IList<Claim> ParseClaims(JwtSecurityToken securityToken)
        {
            var claims = securityToken.Claims.ToList();
            claims.Add(new Claim(ClaimTypes.Name, securityToken.Subject));

            return claims;
        }
    }
}
