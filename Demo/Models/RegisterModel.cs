using System;
using System.ComponentModel.DataAnnotations;

namespace Demo.Models
{
    public class RegisterModel : LoginModel
    {
       
        [Required]
        [EmailAddress]
        public String Email { get; set; }
    }
}

