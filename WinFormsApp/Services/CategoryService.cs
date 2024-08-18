using System.Net.Http;
using System.Text.Json;
using WinFormsApp.Models;
using WinFormsApp.Models.ApiResponse;
using WinFormsApp.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WinFormsApp.Services
{
    public class CategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            try
            {
                string url = $"{Configuration.Configuration.URL}/categories";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var responseData = JsonSerializer.Deserialize<CategoriesResponse>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return responseData?.Data?.Categories ?? new List<Category>();
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show($"Request error: {e.Message}");
                return new List<Category>();
            }
        }
    }
}
