using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp.Models.ApiResponse
{
    [Serializable]
    public class TypeReportResponse
    {
        public TypeReportsData Data { get; set; }

        [Serializable]
        public class TypeReportsData
        {
            public List<TypeReport> TypeReports { get; set; }
            public int? TotalPages { get; set; }
            public int? CurrentPage { get; set; }
        }
    }
}
