using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp.Models
{
    [Serializable]
    public class ExchangeObject
    {
        public int? Id { get; set; }
        public int? ExchangeId { get; set; }
        public int? ObjectId { get; set; }
        public int? UserId { get; set; }
    }
}
