using System.Net.Http;

namespace WinFormsApp.Services
{
    public static class HttpClientFactory
    {
        private static readonly HttpClient _httpClient;

        static HttpClientFactory()
        {
            var primaryHandler = new HttpClientHandler();
            var interceptor = new Interceptor(primaryHandler);
            _httpClient = new HttpClient(interceptor);
        }

        public static HttpClient Instance => _httpClient;
    }
}
