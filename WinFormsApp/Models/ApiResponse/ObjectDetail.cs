using System;
using System.Text.Json.Serialization;

namespace WinFormsApp.Models.ApiResponse
{
    [Serializable]
    public class ObjectDetail
    {
        public int id { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public string image { get; set; }

        private string _status;

        public string status
        {
            get
            {
                return _status switch
                {
                    "Available" => "Disponible",
                    "Removed" => "Retiré",
                    "Deleted" => "Effacé",
                    _ => _status
                };
            }
            set
            {
                _status = value;
            }
        }

        [JsonPropertyName("user")]
        public UserDetail User { get; set; }

        [JsonPropertyName("category")]
        public CategoryDetail category { get; set; }

        public DateTime createdAt { get; set; }

        public DateTime? deletedAt { get; set; }

        public DateTime updatedAt { get; set; }

        public int? userId { get; set; }

        public int? categoryId { get; set; }
    }

    [Serializable]
    public class UserDetail
    {
        public int id { get; set; }

        public string username { get; set; }

        public string email { get; set; }
    }

    [Serializable]
    public class CategoryDetail
    {
        public int id { get; set; }

        public string name { get; set; }
    }
}
