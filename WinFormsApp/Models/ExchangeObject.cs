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
        public int? Exchange_Id { get; set; }
        public int? Object_Id { get; set; }
        public int? User_Id { get; set; }
        public Object Object { get; set; }
    }
}
