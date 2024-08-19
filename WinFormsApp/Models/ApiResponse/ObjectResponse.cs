using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp.Models.ApiResponse
{
    [Serializable]
    public class ObjectResponse
    {
        public ObjectData Data { get; set; }
    }

    [Serializable]
    public class ObjectData
    {
        public List<Object> Objects { get; set; }
        public int? TotalPages { get; set; }
        public int? CurrentPage { get; set; }
    }
}
