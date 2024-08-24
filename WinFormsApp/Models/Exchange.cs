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
        private string status;
        public string Status
        {
            get
            {
                switch (status)
                {
                    case "Accepted":
                        return "Accepté";
                    case "Cancelled":
                        return "Annulé";
                    case "Refused":
                        return "Refusé";
                    case "Proposed":
                        return "Proposé";
                    default:
                        return status;
                }
            }
            set
            {
                status = value;
            }
        }
        public string Note { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string MeetingPlace { get; set; }
        public DateTime? Created_At { get; set; }
        public DateTime? Updated_At { get; set; }
        public DateTime? Date { get; set; }

        public User Proposer { get; set; }
        public User Receiver { get; set; }
        public string ProposerUsername { get; set; }
        public string ReceiverUsername { get; set; }


    }
}
