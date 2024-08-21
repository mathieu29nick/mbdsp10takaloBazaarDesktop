using System.Net.Http;
using System.Text.Json;
using WinFormsApp.Models;
using WinFormsApp.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using WinFormsApp.Models.ApiResponse;

namespace WinFormsApp.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService()
        {
            _httpClient = HttpClientFactory.Instance;
        }

        // Get list of users with pagination and search
        public async Task<UserResponse> GetUsersAsync(int page, int limit, string searchQuery = "", string gender = "", string type = "")
        {
            try
            {
                string url = $"{Configuration.Configuration.URL}/users?page={page}&limit={limit}";

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    url += $"&search={Uri.EscapeDataString(searchQuery)}";
                }
                if (!string.IsNullOrEmpty(gender))
                {
                    url += $"&gender={Uri.EscapeDataString(gender)}";
                }
                if (!string.IsNullOrEmpty(type))
                {
                    url += $"&type={Uri.EscapeDataString(type)}";
                }

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<UserResponse>(responseBody, new JsonSerializerOptions
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

        // Get a user by ID
        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                string url = $"{Configuration.Configuration.URL}/user/{id}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var userNode = JsonSerializer.Deserialize<JsonDocument>(responseBody).RootElement.GetProperty("user");
                return JsonSerializer.Deserialize<User>(userNode.GetRawText(), new JsonSerializerOptions
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

        // Delete a user
        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                string url = $"{Configuration.Configuration.URL}/user/{id}";
                HttpResponseMessage response = await _httpClient.DeleteAsync(url);

                response.EnsureSuccessStatusCode();
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show($"Erreur de requête : {e.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Update a user
        public async Task<bool> UpdateUserAsync(User user, string base64Image = null)
        {
            try
            {
                string url = $"{Configuration.Configuration.URL}/user/{user.Id}";
                var requestPayload = new Dictionary<string, object>
                {
                    { "username", user.Username },
                    { "email", user.Email },
                    { "first_name", user.FirstName },
                    { "last_name", user.LastName },
                    { "gender", user.Gender }
                };

                if (!string.IsNullOrEmpty(base64Image))
                {
                    requestPayload.Add("image", base64Image);
                }

                string jsonPayload = JsonSerializer.Serialize(requestPayload);
                HttpContent content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

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
    }
}
