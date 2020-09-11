using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Net_Core_3._0_Web_API.ApplicationCore.Entities
{
    public class Image : BaseEntity
    {
        public string ImageId { get; set; }
        public string FileName { get; set; }
    }
}
