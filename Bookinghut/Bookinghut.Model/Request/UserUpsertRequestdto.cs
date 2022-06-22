using System;
using System.Collections.Generic;
using System.Text;

namespace Bookinghut.Model.Request
{
    public class UserUpsertRequestdto
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
        public string Adress { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public string Role { get; set; }
    }
}
