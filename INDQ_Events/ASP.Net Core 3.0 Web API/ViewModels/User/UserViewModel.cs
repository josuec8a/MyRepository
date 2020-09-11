using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Net_Core_3._0_Web_API.ViewModels.User
{
    public class UserViewModel
    {
        [Required]
        [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "Solo letras")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "Solo letras")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Dirección de Email inválida")]
        public string Email { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "La Contraseña debe contener al menos 8 caracteres")]
        public string Password { get; set; }
        [Required]
        public string Gender { get; set; }
    }
}
