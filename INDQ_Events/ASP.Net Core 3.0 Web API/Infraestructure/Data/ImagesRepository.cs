using ASP.Net_Core_3._0_Web_API.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Net_Core_3._0_Web_API.Infraestructure.Data
{
    public class ImagesRepository : EfRepository<Image>
    {
        public ImagesRepository(CatalogContext context) : base(context) { }

    }
}
