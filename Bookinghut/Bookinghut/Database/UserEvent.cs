using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bookinghut.Database
{
    public class UserEvent
    {
        [Key]

        public int UserEventID { get; set; }
        [ForeignKey("EventID")]

        public int EventID { get; set; }
        public Event Event { get; set; }
        [ForeignKey("UserID")]

        public int UserID { get; set; }
        public User User { get; set; }

        public float TotalCost { get; set; }
    }
}
