using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Net_Core_3._0_Web_API.ApplicationCore.Exceptions
{
    public class AttendanceExistsException : Exception
    {

        public AttendanceExistsException()
        {

        }

        public AttendanceExistsException(string message)
            : base(message)
        {

        }
    }

    public class AttendenceNotExistsException : Exception
    {
        public AttendenceNotExistsException()
        {

        }

        public AttendenceNotExistsException(string message)
            : base(message)
        {

        }
    }
}
