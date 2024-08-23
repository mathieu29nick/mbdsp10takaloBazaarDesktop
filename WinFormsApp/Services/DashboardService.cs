using System.Net.Http;
using System.Text.Json;
using WinFormsApp.Models;
using System.Web;

namespace WinFormsApp.Services
{
    public class DashboardService
    {
        private readonly HttpClient _httpClient;

        public DashboardService()
        {
            _httpClient = HttpClientFactory.Instance;
        }

        public async Task<Dashboard> GetDashboardAsync(string date1, string date2, string statut)
        {
            try
            {
                string baseUrl = $"{Configuration.Configuration.URL}/dashboard";

                var uriBuilder = new UriBuilder(baseUrl);
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);

                if (!string.IsNullOrEmpty(date1))
                {
                    query["date1"] = date1;
                }
                if (!string.IsNullOrEmpty(date2))
                {
                    query["date2"] = date2;
                }
                if (!string.IsNullOrEmpty(statut))
                {
                    query["status"] = statut;
                }

                uriBuilder.Query = query.ToString();
                string url = uriBuilder.ToString();
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<Dashboard>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show($"Request error: {e.Message}");
                return null;
            }
        }

    }
}
