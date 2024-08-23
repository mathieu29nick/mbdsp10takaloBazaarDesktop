using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WinFormsApp.Configuration;
using WinFormsApp.Models.ApiResponse;

namespace WinFormsApp.Services
{
    public class ReportService
    {
        private readonly HttpClient _httpClient;

        public ReportService()
        {
            _httpClient = HttpClientFactory.Instance;
        }

        public async Task<ReportResponse> GetReportsAsync(int page, int limit)
        {
            var url = $"{Configuration.Configuration.URL}/reports?page={page}&limit={limit}";
            var response = await _httpClient.GetStringAsync(url);
            var jsonResponse = JsonSerializer.Deserialize<JsonDocument>(response);

            var dataNode = jsonResponse.RootElement.GetProperty("data");

            var reports = JsonSerializer.Deserialize<List<ReportItem>>(dataNode.GetProperty("reports").GetRawText());
            var reportResponse = new ReportResponse
            {
                Reports = reports,
                TotalItems = dataNode.GetProperty("totalItems").GetInt32(),
                TotalPages = dataNode.GetProperty("totalPages").GetInt32(),
                CurrentPage = dataNode.GetProperty("currentPage").GetInt32()
            };

            return reportResponse;
        }

        public async Task<ReportDetailResponse> GetReportDetailsAsync(int objectId, int page = 1, string createdAtStart = null, string createdAtEnd = null, string reason = null)
        {
            var uriBuilder = new UriBuilder($"{Configuration.Configuration.URL}/object/{objectId}/reports");

            var query = new List<string>();
            query.Add($"page={page.ToString()}");
            if (!string.IsNullOrEmpty(createdAtStart)) query.Add($"created_at_start={createdAtStart}");
            if (!string.IsNullOrEmpty(createdAtEnd)) query.Add($"created_at_end={createdAtEnd}");
            if (!string.IsNullOrEmpty(reason)) query.Add($"reason={reason}");

            uriBuilder.Query = string.Join("&", query);

            var response = await _httpClient.GetStringAsync(uriBuilder.ToString());
            var reportDetailResponse = JsonSerializer.Deserialize<ReportDetailResponse>(response);

            return reportDetailResponse;
        }

    }
}
