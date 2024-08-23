using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WinFormsApp.Models
{
    [Serializable]
    public class Report
    {
        public int id { get; set; }
        public int objectId { get; set; }
        public int reporterUserId { get; set; }
        public string reason { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public User User { get; set; }
    }
}
