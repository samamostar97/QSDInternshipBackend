using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bookinghut.Model.Request
{
    public class AuthenticateRequest
    {
        [Required]
        [EmailAddress]
        public string Mail { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
