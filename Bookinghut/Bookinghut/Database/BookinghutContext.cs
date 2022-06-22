using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookinghut.Database
{
    public partial class BookinghutContext : DbContext
    {
        public BookinghutContext(DbContextOptions<BookinghutContext> options)
         : base(options)
        {
        }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<UserEvent> UserEvent { get; set; }
        public virtual DbSet<Venue> Venue { get; set; }
        //public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
         
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}