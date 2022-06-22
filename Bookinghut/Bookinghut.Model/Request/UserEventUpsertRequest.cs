using System;
using System.Collections.Generic;
using System.Text;

namespace Bookinghut.Model.Request
{
    public class UserEventUpsertRequest
    {
        public int UserEventID { get; set; } 
        public int EventID { get; set; }
        public int UserID { get; set; }
        public float TotalCost { get; set; }
    }
}
