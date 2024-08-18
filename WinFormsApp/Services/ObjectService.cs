using System.Net.Http;
using System.Text.Json;
using WinFormsApp.Models;
using WinFormsApp.Models.ApiResponse;
using WinFormsApp.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WinFormsApp.Services
{
    public class ObjectService
    {
        private readonly HttpClient _httpClient;

        public ObjectService()
        {
            _httpClient = new HttpClient();
        }
    }
}
