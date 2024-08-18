using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp.Models
{
    [Serializable]
    public class Report
    {
        public int Id { get; set; }
        public int ObjectId { get; set; }
        public int ReporterUserId { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
