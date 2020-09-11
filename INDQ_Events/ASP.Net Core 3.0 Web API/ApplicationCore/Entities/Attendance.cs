using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Net_Core_3._0_Web_API.ApplicationCore.Entities
{
    public class Attendance : BaseEntity
    {
        public int EventId { get; set; }
        public string UserName { get; set; }
        //public int UserId { get; set; }

        public Event Event { get; set; }
        //public User User { get; set; }
    }
}
