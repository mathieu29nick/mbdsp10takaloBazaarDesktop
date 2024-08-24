using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WinFormsApp.Models.ApiResponse
{
    [Serializable]
    public class ReportResponse
    {
        public List<ReportItem> Reports { get; set; }

        public int TotalItems { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }
    }
}
