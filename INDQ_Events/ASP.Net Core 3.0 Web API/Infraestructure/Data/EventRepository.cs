using ASP.Net_Core_3._0_Web_API.ApplicationCore.Entities;
using ASP.Net_Core_3._0_Web_API.ApplicationCore.Exceptions;
using ASP.Net_Core_3._0_Web_API.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Net_Core_3._0_Web_API.Infraestructure.Data
{
    public class EventRepository : EfRepository<Event>, IEventRepository
    {
        public EventRepository(CatalogContext context) : base(context) { }

        public async Task ConfirmAttendace(Attendance attendance,
            IAsyncRepository<Attendance> attendaceRepository)
        {
            var userHadConfirmed =
                attendaceRepository.GetWhere(exp => exp.EventId == attendance.EventId && exp.UserName == attendance.UserName).Result.Any();

            if (userHadConfirmed)
                throw new AttendanceExistsException();
            
            await attendaceRepository.Add(attendance);
        }

        public async Task CancelAttendace(Attendance attendance,
            IAsyncRepository<Attendance> attendaceRepository)
        {
            var item = attendaceRepository.FirstOrDefault(exp => exp.EventId == attendance.EventId && exp.UserName == attendance.UserName).Result;
            if (item == null)
                throw new AttendenceNotExistsException();

            await attendaceRepository.Remove(item);
        }
    }
}
