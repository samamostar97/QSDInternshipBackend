using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bookinghut.Model.Request
{
    public class RegisterRequest
    {
    
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Mail { get; set; }

        [Required]
        public string Phone { get; set; }
        
        [Required]
        public string Adress { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Range(typeof(bool), "true", "true")]
        public bool AcceptTerms { get; set; }
    }
}
