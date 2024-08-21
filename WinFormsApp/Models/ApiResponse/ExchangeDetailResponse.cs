using System;
using System.Collections.Generic;
using WinFormsApp.Models;

namespace WinFormsApp.Models.ApiResponse
{
    [Serializable]
    public class ExchangeDetailResponse
    {
        public int Id { get; set; }
        public int ProposerUserId { get; set; }
        public int ReceiverUserId { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public DateTime? Appointment_Date { get; set; }
        public string Meeting_Place { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
        public DateTime? Date { get; set; }
        public User Proposer { get; set; }
        public User Receiver { get; set; }
        public List<ExchangeObject> Exchange_Objects { get; set; }
    }
}
