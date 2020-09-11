using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Net_Core_3._0_Web_API.ApplicationCore.Entities
{
    public class Event : BaseEntity
    {
        public string EventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Image { get; set; }
        public int Attendances { get; set; }
        public bool WillYouAttend { get; set; }
        public Point Location { get; set; }
    }
}
