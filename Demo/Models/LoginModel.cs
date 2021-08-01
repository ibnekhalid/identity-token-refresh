using System;
using System.ComponentModel.DataAnnotations;

namespace Demo.Models
{
    public class LoginModel
    {
        [Required]
        public String Username { get; set; }

        [Required]
        public String Password { get; set; }
    }
}

