using System.Net.Http;

namespace WinFormsApp.Services
{
    public static class HttpClientFactory
    {
        private static readonly HttpClient _httpClient;
        private static readonly Interceptor interceptor;

        static HttpClientFactory()
        {
            var primaryHandler = new HttpClientHandler();
            interceptor = new Interceptor(primaryHandler);
            _httpClient = new HttpClient(interceptor);
        }

        public static HttpClient Instance => _httpClient;
    }
}
