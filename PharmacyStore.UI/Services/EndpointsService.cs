namespace PharmacyStore.UI.Services
{
    public static class EndpointsService
    {
        public static string BaseUrl = "https://localhost:44323/";
        public static string CompanyEndpoint = $"{BaseUrl}api/company/";
        public static string ProductEndpoint = $"{BaseUrl}api/product/";
        public static string RegistrationEndpoint = $"{BaseUrl}api/user/register/";
        public static string LoginEndpoint = $"{BaseUrl}api/user/login/";
    }
}
