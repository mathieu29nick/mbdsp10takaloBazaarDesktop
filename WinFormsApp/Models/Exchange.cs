using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp.Models
{
    [Serializable]
    public class Exchange
    {
        public int? Id { get; set; }
        public int? ProposerUserId { get; set; }
        public int? ReceiverUserId { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string MeetingPlace { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? Date { get; set; }
    }
}
