using System;
using System.Collections.Generic;
using System.Text;

namespace Bookinghut.Model
{
   public class MUser
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
        public string Adress { get; set; }
        public string Mail { get; set; }
        public string Role { get; set; }
        //public ICollection<MUserRole> UserRole { get; set; }

    }
}
