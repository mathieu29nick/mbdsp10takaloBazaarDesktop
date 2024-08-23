using System.Text.Json.Serialization;

namespace WinFormsApp.Models.ApiResponse
{
    [Serializable]
    public class ReportDetailResponse
    {
        [JsonPropertyName("object")]
        public ObjectDetail Object { get; set; }
        public List<Report> reports { get; set; }
        public int totalPages { get; set; }
        public string currentPage { get; set; }
    }
}