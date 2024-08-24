using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp.Configuration
{
    public static class Configuration
    {
        public static string URL { get; } = "https://takalobazarserver.onrender.com/api";

        //public static string URL { get; } = "http://localhost:3000/api";
        public static string TOKKEN { get; set; } = string.Empty;
    }
}
