using ASP.Net_Core_3._0_Web_API.ApplicationCore.Entities;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ASP.Net_Core_3._0_Web_API.ApplicationCore.Specifications
{
    public class EventFilterPaginatedSpecification : BaseSpecification<Event>
    {
        public EventFilterPaginatedSpecification(int skip, int take, Expression<Func<Event, bool>> criteria)
        : base(criteria)
        {
            ApplyPaging(skip, take);
        }
    }
}
