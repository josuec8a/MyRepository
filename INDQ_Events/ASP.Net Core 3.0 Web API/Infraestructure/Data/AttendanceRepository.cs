using ASP.Net_Core_3._0_Web_API.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Net_Core_3._0_Web_API.Infraestructure.Data
{
    public class AttendanceRepository : EfRepository<Attendance>
    {
        public AttendanceRepository(CatalogContext context) : base(context) { }
    }
}
