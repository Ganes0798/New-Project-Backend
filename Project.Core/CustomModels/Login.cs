using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Project.Core.Enums.CommonEnums;

namespace Project.Core.CustomModels
{
    public class Login
    {
        [Key]
        public int User_id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public Roles RoleName { get; set; }


        public bool termAccept { get; set; }

        public DateTime createdOn { get; set; }
    }

    public class Signin
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "password", Description = "Enter Valid Password")]
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
        public bool Rememberme { get; set; }
    }
}
