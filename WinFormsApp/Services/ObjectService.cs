﻿using System.Net.Http;
using System.Text.Json;
using WinFormsApp.Models;
using WinFormsApp.Models.ApiResponse;
using WinFormsApp.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp.Services
{
    public class ObjectService
    {
        private readonly HttpClient _httpClient;

        public ObjectService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Models.Object>> GetObjectsAsync(int page, int limit, string name = null, string description = null, int? userId = null, int? categoryId = null, string status = null)
        {
            try
            {
                string url = $"{Configuration.Configuration.URL}/objects?page={page}&limit={limit}";
                if (!string.IsNullOrEmpty(name))
                    url += $"&name={name}";

                if (!string.IsNullOrEmpty(description))
                    url += $"&description={description}";

                if (userId.HasValue)
                    url += $"&user_id={userId}";

                if (categoryId.HasValue)
                    url += $"&category_id={categoryId}";

                if (!string.IsNullOrEmpty(status))
                    url += $"&status={status}";

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var responseData = JsonSerializer.Deserialize<ObjectResponse>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return responseData?.Data?.Objects ?? new List<Models.Object>();
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show($"Request error: {e.Message}");
                return new List<Models.Object>();
            }
        }
    }
}
