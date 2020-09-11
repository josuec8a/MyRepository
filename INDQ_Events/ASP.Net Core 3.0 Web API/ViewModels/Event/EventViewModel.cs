using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Net_Core_3._0_Web_API.ViewModels.Event
{
    public class EventViewModelResponse
    {
        public int Page { get; set; }
        public int Pages { get; set; }
        public IEnumerable<EventViewModel> Items { get; set; }
    }

    public class EventViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Image { get; set; }
        public int Attendances { get; set; }
        public bool WillYouAttend { get; set; }
    }
}
