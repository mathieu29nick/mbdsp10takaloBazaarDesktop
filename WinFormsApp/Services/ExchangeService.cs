using System.Net.Http;
using System.Text.Json;
using WinFormsApp.Models;
using WinFormsApp.Models.ApiResponse;
using WinFormsApp.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WinFormsApp.Services
{
    public class ExchangeService
    {
        private readonly HttpClient _httpClient;

        public ExchangeService()
        {
            _httpClient = HttpClientFactory.Instance;
        }

        public async Task<List<Exchange>> GetExchangesAsync(int page = 1, int limit = 10, string orderBy = null, string orderDirection = null, Dictionary<string, string> filters = null)
        {
            try
            {
                string url = $"{Configuration.Configuration.URL}/exchanges?page={page}&limit={limit}";

                if (!string.IsNullOrEmpty(orderBy))
                    url += $"&orderBy={orderBy}";

                if (!string.IsNullOrEmpty(orderDirection))
                    url += $"&orderDirection={orderDirection}";

                if (filters != null)
                {
                    foreach (var filter in filters)
                    {
                        url += $"&{filter.Key}={filter.Value}";
                    }
                }

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var responseData = JsonSerializer.Deserialize<ExchangeResponse>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (responseData != null && responseData.Exchanges != null)
                {
                    foreach (var exchange in responseData.Exchanges)
                    {
                        exchange.ProposerUsername = exchange.Proposer?.Username;
                        exchange.ReceiverUsername = exchange.Receiver?.Username;
                    }

                    return responseData.Exchanges;
                }
                else
                {
                    MessageBox.Show("Aucun échange trouvé ou problème de désérialisation.");
                    return new List<Exchange>();
                }
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show($"Erreur de requête : {e.Message}");
                return new List<Exchange>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
                return new List<Exchange>();
            }
        }


    }
}
