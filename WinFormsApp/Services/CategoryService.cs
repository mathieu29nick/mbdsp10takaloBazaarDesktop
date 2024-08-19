using System.Net.Http;
using System.Text.Json;
using WinFormsApp.Models;
using WinFormsApp.Models.ApiResponse;
using WinFormsApp.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System;

namespace WinFormsApp.Services
{
    public class CategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService()
        {
            _httpClient = HttpClientFactory.Instance;
        }

        // Get list of categories with pagination and search
        public async Task<CategoriesResponse> GetCategoriesAsync(int page, int limit, string searchQuery = "")
        {
            try
            {
                string url = $"{Configuration.Configuration.URL}/categories?page={page}&limit={limit}";

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    url += $"&q={Uri.EscapeDataString(searchQuery)}";
                }

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<CategoriesResponse>(responseBody, new JsonSerializerOptions
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

        // Get a category by ID
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            try
            {
                string url = $"{Configuration.Configuration.URL}/category/{id}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var categoryNode = JsonSerializer.Deserialize<JsonDocument>(responseBody).RootElement.GetProperty("category");
                return JsonSerializer.Deserialize<Category>(categoryNode.GetRawText(), new JsonSerializerOptions
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

        // Update a category
        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            try
            {
                string url = $"{Configuration.Configuration.URL}/categories/{category.Id}";
                var requestPayload = new Dictionary<string, string>
                {
                    { "name", category.Name }
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

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                string url = $"{Configuration.Configuration.URL}/category/{id}";
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


        // Save a new category
        public async Task<bool> SaveCategoryAsync(Category category)
        {
            try
            {
                string url = $"{Configuration.Configuration.URL}/categories";
                var requestPayload = new Dictionary<string, string>
                {
                    { "category", category.Name }
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
