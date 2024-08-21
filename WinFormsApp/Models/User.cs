using System;
using System.Text.Json.Serialization;

namespace WinFormsApp.Models
{
    [Serializable]
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("profile_picture")]
        public string ProfilePicture { get; set; }

        private string _gender;
        public string Gender
        {
            get
            {
                return _gender?.Equals("Male", StringComparison.OrdinalIgnoreCase) == true ? "Homme" :
                       _gender?.Equals("Female", StringComparison.OrdinalIgnoreCase) == true ? "Femme" :
                       _gender;
            }
            set
            {
                _gender = value;
            }
        }

        public string Type { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        private string _status;
        public string Status
        {
            get
            {
                if (DeletedAt.HasValue)
                {
                    return "Supprimé";
                }
                return string.Equals(_status, "Available", StringComparison.OrdinalIgnoreCase) ? "Actif" : _status;
            }
            set
            {
                _status = value;
            }
        }
    }
}
