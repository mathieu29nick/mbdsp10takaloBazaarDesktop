using System.Net.Http;
using System.Text.Json;
using WinFormsApp.Models;
using WinFormsApp.Models.ApiResponse;
using WinFormsApp.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;

namespace WinFormsApp.Services
{
    public class ObjectService
    {
        private readonly HttpClient _httpClient;

        public ObjectService()
        {
            _httpClient = HttpClientFactory.Instance;
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
                    if(categoryId==-1) url += $"&category_id=";
                    else url += $"&category_id={categoryId}";

                if (!string.IsNullOrEmpty(status))
                    url += $"&status={status}";

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var responseData = JsonSerializer.Deserialize<ObjectResponse>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (responseData != null && responseData.Data != null)
                {
                    foreach (var obj in responseData.Data.Objects)
                    {
                        obj.CategoryName = obj.Category?.Name;
                    }
                }

                return responseData?.Data?.Objects ?? new List<Models.Object>();
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show($"Request error: {e.Message}");
                return new List<Models.Object>();
            }
        }

        public async Task<bool> CreateObjectAsync(Models.Object newObject)
        {
            try
            {
                string url = $"{Configuration.Configuration.URL}/objects";

                var jsonObject = new
                {
                    name = newObject.Name,
                    description = newObject.Description,
                    category_id = newObject.CategoryId,
                    image_file = newObject.Image,
                    user_id = newObject.UserId
                };

                string json = JsonSerializer.Serialize(jsonObject);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erreur lors de la création de l'objet : {errorResponse}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la création de l'objet : {ex.Message}");
                return false;
            }
            }

        public async Task<Models.Object> GetObjectByIdAsync(int objectId)
        {
            try
            {
                string url = $"{Configuration.Configuration.URL}/object/{objectId}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                
                var responseData = JsonSerializer.Deserialize<Models.Object>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (responseData == null)
                {
                    MessageBox.Show("Problème de désérialisation ou structure de données inattendue.");
                    return null;
                }

                return responseData;
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show($"Erreur de requête : {e.Message}");
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
                return null;
            }
        }



        public async Task<bool> UpdateObjectAsync(int objectId, Models.Object updatedObject)
        {
            try
            {
                string url = $"{Configuration.Configuration.URL}/objects/{objectId}";

                var jsonObject = new
                {
                    name = updatedObject.Name,
                    description = updatedObject.Description,
                    category_id = updatedObject.CategoryId,
                    image_file = updatedObject.Image
                };

                string json = JsonSerializer.Serialize(jsonObject);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erreur lors de la modification de l'objet : {errorResponse}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la modification de l'objet : {ex.Message}");
                return false;
            }
        }

    }

}
