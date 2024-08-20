using System;
using System.Collections.Generic;

namespace WinFormsApp.Models.ApiResponse
{
    [Serializable]
    public class UserResponse
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public List<User> Users { get; set; }
    }
}
