using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp.Models
{
    [Serializable]
    public class Dashboard
    {
        public int OngoingExchanges { get; set; }
        public int AcceptedExchanges { get; set; }
        public int RefusedExchanges { get; set; }
        public int CancelledExchanges { get; set; }
        public List<ObjectByCategory> ObjectsByCategory { get; set; }
        public List<ExchangeBetweenDates> ExchangesBetweenDates { get; set; }
        public List<ExchangeByUser> ExchangesByUser { get; set; }

        [Serializable]
        public class ObjectByCategory
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int ObjectCount { get; set; }
        }

        [Serializable]
        public class ExchangeBetweenDates
        {
            public string Type { get; set; }
            public string Period { get; set; }
            public int ExchangeCount { get; set; }
        }

        [Serializable]
        public class ExchangeByUser
        {
            public int UserId { get; set; }
            public string Username { get; set; }
            public int ExchangeCount { get; set; }
            public double Percentage { get; set; }
        }
    }
}
