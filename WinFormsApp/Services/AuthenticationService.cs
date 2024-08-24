using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

namespace WinFormsApp.Services
{
    public class AuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly Interceptor _interceptor;
        private const string TokenFilePath = "token.txt"; // File to store the token

        public AuthenticationService(Interceptor interceptor)
        {
            _httpClient = HttpClientFactory.Instance;
            _interceptor = interceptor;

            // Load token from file if it exists
            if (File.Exists(TokenFilePath))
            {
                Configuration.Configuration.TOKKEN = File.ReadAllText(TokenFilePath);
            }
        }

        public async Task<bool> Login(string username, string password)
        {
            var url = $"{Configuration.Configuration.URL}/auth/admin/login";
            var payload = new { username, password };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var jsonNode = JsonDocument.Parse(json).RootElement;

                    var token = jsonNode.GetProperty("admin").GetProperty("token").GetString();

                    // Save the token to a file
                    File.WriteAllText(TokenFilePath, token);

                    Configuration.Configuration.TOKKEN = token;
                    return true;
                }
                else
                {
                    var errorMessage = await ExtractErrorMessage(response);
                    throw new Exception(errorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                var errorMessage = $"Erreur HTTP : {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $" Détails : {ex.InnerException.Message}";
                }
                throw new Exception(errorMessage, ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
        }

        public async Task Logout()
        {
            var url = $"{Configuration.Configuration.URL}/auth/logout";
            try
            {
                await _httpClient.PostAsync(url, null);
            }
            finally
            {
                Configuration.Configuration.TOKKEN = string.Empty;
                if (File.Exists(TokenFilePath))
                {
                    File.Delete(TokenFilePath);
                }
            }
        }

        private async Task<string> ExtractErrorMessage(HttpResponseMessage response)
        {
            try
            {
                var json = await response.Content.ReadAsStringAsync();
                var jsonNode = JsonDocument.Parse(json).RootElement;
                return jsonNode.GetProperty("error").GetString() ?? "An unexpected error occurred.";
            }
            catch
            {
                return "An unexpected error occurred while processing the error response.";
            }
        }
    }
}
