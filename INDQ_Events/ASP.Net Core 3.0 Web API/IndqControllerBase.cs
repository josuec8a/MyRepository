using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Net_Core_3._0_Web_API
{
    public class IndqControllerBase : ControllerBase
    {
        [NonAction]
        public virtual void LogException(Exception ex)
        {
            //log exception code...
        }
    }
}
