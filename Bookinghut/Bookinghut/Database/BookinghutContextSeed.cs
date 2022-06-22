using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Bookinghut.Database
{
    public partial class BookinghutContext
    {
        public static string GenerateSalt()
        {
            var buf = new byte[16];
            (new RNGCryptoServiceProvider()).GetBytes(buf);
            return Convert.ToBase64String(buf);
        }
        public static string GenerateHash(string salt, string password)
        {
            byte[] src = Convert.FromBase64String(salt);
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] dst = new byte[src.Length + bytes.Length];

            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);

            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] inArray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inArray);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {

            List<string> Salt = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                Salt.Add(GenerateSalt());
            }
            //modelBuilder.Entity<Role>().HasData
            //(
            //    new Role { RoleID = 1, Name = "Admin" },
            //    new Role { RoleID = 2, Name = "Organizer" },
            //    new Role { RoleID = 3, Name = "Customer" }

            //    );
            modelBuilder.Entity<User>().HasData
              (
                  new User
                  {
                      UserID = 1,
                      FirstName = "Admin",
                      LastName = "Admin",
                      Mail = "admin.admin@gmail.com",
                      Adress = "adressadmin",
                      Phone = "000111222",
                      //RoleID = 1
                  }
                  );
            modelBuilder.Entity<Venue>().HasData
       (
           new Venue
           {
               VenueID = 1,
               Name = "Venue1"
           }
           );
            modelBuilder.Entity<Event>().HasData
     (
         new Event
         {
             EventID = 1,
             Name = "Name>Event",
             Price = 12,
             VenueID = 1,
             NumberOfTickets = 2,
             Status = true
         }
         );
        }
    }
}
