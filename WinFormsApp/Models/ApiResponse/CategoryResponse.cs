using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp.Models.ApiResponse
{
    [Serializable]
    public class CategoriesResponse
    {
        public CategoriesData Data { get; set; }
    }

    [Serializable]
    public class CategoriesData
    {
        public List<Category> Categories { get; set; }
        public int? TotalPages { get; set; }
        public int? CurrentPage { get; set; }
    }
}
