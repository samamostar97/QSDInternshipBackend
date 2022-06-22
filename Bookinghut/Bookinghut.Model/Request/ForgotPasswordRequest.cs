using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bookinghut.Model.Request
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Mail { get; set; }
    }
}
