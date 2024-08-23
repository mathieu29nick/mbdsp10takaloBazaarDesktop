using System;
using System.Text.Json.Serialization;

namespace WinFormsApp.Models.ApiResponse
{
    [Serializable]
    public class ReportItem
    {
        [JsonPropertyName("object_id")]
        public int ObjectId { get; set; }

        public string reportCount { get; set; }

        [JsonPropertyName("object_name")]
        public string ObjectName { get; set; }

        [JsonPropertyName("Object")]
        public ObjectDetail ObjectDetails { get; set; }
    }
}
