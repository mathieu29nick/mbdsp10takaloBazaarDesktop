using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp.Models.ApiResponse
{
    [Serializable]
    public class ExchangeResponse
    {
        public List<Exchange> Exchanges { get; set; }
        public int? TotalPages { get; set; }
        public string? CurrentPage { get; set; }
    }

    
    
}
