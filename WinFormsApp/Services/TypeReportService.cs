using System.Net.Http;
using System.Text.Json;
using WinFormsApp.Models;
using WinFormsApp.Models.ApiResponse;
using WinFormsApp.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace WinFormsApp.Services
{
    public class TypeReportService
    {
        private readonly HttpClient _httpClient;

        public TypeReportService()
        {
            _httpClient = HttpClientFactory.Instance;
        }

        // Get list of typeReports with pagination and search
        public async Task<TypeReportResponse> GetTypeReportsAsync(int page, int limit, string searchQuery = "")
        {
            try
            {
                string url = $"{Configuration.Configuration.URL}/typeReports?page={page}&limit={limit}";
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    url += $"&q={Uri.EscapeDataString(searchQuery)}";
                }

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<TypeReportResponse>(responseBody, new JsonSerializerOptions
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


        // Update a typeReport
        public async Task<bool> UpdateTypeReportAsync(TypeReport typeReport)
        {
            try
            {
                string url = $"{Configuration.Configuration.URL}/typeReports/{typeReport.Id}";
                var requestPayload = new Dictionary<string, string>
                {
                    { "name", typeReport.Name }
                };

                string jsonPayload = JsonSerializer.Serialize(requestPayload);
                HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync(url, content);

                response.EnsureSuccessStatusCode();
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show($"Request error: {e.Message}");
                return false;
            }
        }

        // Delete a typeReport
        public async Task<bool> DeleteTypeReportAsync(int id)
        {
            try
            {
                string url = $"{Configuration.Configuration.URL}/typeReport/{id}";
                HttpResponseMessage response = await _httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    // Get the error message from the response body
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonSerializer.Deserialize<JsonDocument>(responseBody);
                    string errorMessage = jsonResponse.RootElement.GetProperty("message").GetString();

                    // Display the error message in a MessageBox
                    MessageBox.Show(errorMessage, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show($"Erreur de requête : {e.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        // Save a new typeReport
        public async Task<bool> SaveTypeReportAsync(TypeReport typeReport)
        {
            try
            {
                string url = $"{Configuration.Configuration.URL}/typeReports";
                var requestPayload = new Dictionary<string, string>
                {
                    { "name", typeReport.Name }
                };

                string jsonPayload = JsonSerializer.Serialize(requestPayload);
                HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                response.EnsureSuccessStatusCode();
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show($"Request error: {e.Message}");
                return false;
            }
        }
    }
}
