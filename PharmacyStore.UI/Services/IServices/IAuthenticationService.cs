using PharmacyStore.UI.Models.ViewModels;
using System.Threading.Tasks;

namespace PharmacyStore.UI.Services.IServices
{
    public interface IAuthenticationService
    {
        public Task<bool> Registration(RegistrationViewModel model);
        public Task<bool> Login(LoginViewModel model);
        public Task Logout();
    }
}
