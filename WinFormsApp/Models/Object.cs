﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp.Models
{
    [Serializable]
    public class Object
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
        private string status;
        public string Status
        {
            get
            {
                switch (status)
                {
                    case "Available":
                        return "Disponible";
                    case "Removed":
                        return "Retiré";
                    default:
                        return status;
                }
            }
            set
            {
                status = value;
            }
        }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public DateTime? DeletedAt { get; set; }

        public string CategoryName { get; set; }
        public Category Category { get; set; }

        public string UserName { get; set; }
        public User User { get; set; }
    }
}
