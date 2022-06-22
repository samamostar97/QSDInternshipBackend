using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bookinghut.Database
{
    public class User
    {
        [Key]

        public int UserID { get; set; }
        public string FirstName { get; set; }  
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
        public string Adress { get; set; }
        public string Mail { get; set; }
        public string PasswordHash { get; set; }
        public bool AcceptTerms { get; set; }
        //public string PasswordSalt { get; set; }
        //public virtual ICollection<UserRole> UserRole { get; set; }

        public Role Role { get; set; }
        public string VerificationToken { get; set; }
        public DateTime? Verified { get; set; } = DateTime.Now;
        public bool IsVerified => Verified.HasValue || PasswordReset.HasValue;
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public DateTime? PasswordReset { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }

        public bool OwnsToken(string token)
        {
            return this.RefreshTokens?.Find(x => x.Token == token) != null;
        }

    }
}
