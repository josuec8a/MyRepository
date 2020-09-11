using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Net_Core_3._0_Web_API.ViewModels.Event
{
    public class CreateViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Image { get; set; }
        [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Solo permite archivos con extencion jpg, jpeg, png")]
        public IFormFile ImageFile { get; set; }
        [Required]
        public int Attendances { get; set; }
        [Required]
        public bool WillYouAttend { get; set; }
        [Required]
        public decimal[] Location { get; set; }
    }
}
