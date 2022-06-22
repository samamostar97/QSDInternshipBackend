using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bookinghut.Database
{
    public class Event
    {
        [Key]
        public int EventID { get; set; }
        public string Name { get; set; }
        [ForeignKey("VenueID")]

        public int VenueID { get; set; }
        public Venue Venue { get; set; }
        public DateTime Timing { get; set; }
        public float Price { get; set; }
        public int NumberOfTickets { get; set; }
        public bool Status {get;set;}
    }
}
