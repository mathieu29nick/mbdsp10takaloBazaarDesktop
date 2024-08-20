using System.Net.Http;
using System.Text.Json;
using WinFormsApp.Models;
using WinFormsApp.Models.ApiResponse;
using WinFormsApp.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp.Configuration;

namespace WinFormsApp.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService()
        {
            _httpClient =  HttpClientFactory.Instance;
        }

        public async Task<UserResponse> GetUsersAsync(int page, int limit)
        {
            try
            {
                string url = $"{Configuration.Configuration.URL}/users?page={page}&limit={limit}";

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var responseData = JsonSerializer.Deserialize<UserResponse>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (responseData?.Users == null)
                {
                    MessageBox.Show("Problème de désérialisation ou structure de données inattendue.");
                }

                return responseData ?? new UserResponse { Users = new List<User>() };
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show($"Request error: {e.Message}");
                return new UserResponse { Users = new List<User>() };
            }
        }



    }
}
