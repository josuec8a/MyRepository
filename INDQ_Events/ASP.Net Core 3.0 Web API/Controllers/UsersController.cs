using ASP.Net_Core_3._0_Web_API.Infraestructure.Data.Identity;
using ASP.Net_Core_3._0_Web_API.ViewModels.Login;
using ASP.Net_Core_3._0_Web_API.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ASP.Net_Core_3._0_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : IndqControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("/users")]
        public async Task<ActionResult> CreateUser([FromBody] UserViewModel userVm)
        {
            try
            {
                ApplicationUser applicationUser = null;
                applicationUser = await _userManager.FindByEmailAsync(userVm.Email);

                if (applicationUser != null)
                    return StatusCode(403, "La cuenta con ese correo electrónico ya existe");

                var user = new ApplicationUser
                {
                    UserName = userVm.Email,
                    Email = userVm.Email,
                    FirstName = userVm.FirstName,
                    LastName = userVm.LastName,
                    //Password = userVm.Password,
                    Gender = userVm.Gender
                };

                var result = await _userManager.CreateAsync(user, userVm.Password);
                if (!result.Succeeded)
                    return StatusCode(500, "Error al crear usuario");

                applicationUser = await _userManager.FindByEmailAsync(user.Email);

                return Ok(applicationUser.Id);
            }
            catch (Exception ex)
            {
                LogException(ex);
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        [Route("/users/login")]
        public async Task<ActionResult<LoginResponseViewModel>> Login([FromBody] LoginViewModel loginVm)
        {
            var result = await _signInManager.PasswordSignInAsync(loginVm.Email, loginVm.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var usuario = await _userManager.FindByEmailAsync(loginVm.Email);
                var roles = await _userManager.GetRolesAsync(usuario);
                return BuildToken(usuario, roles);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Credenciales inválidas");
                return BadRequest(ModelState);
            }
        }

        private LoginResponseViewModel BuildToken(ApplicationUser applicationUser, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, applicationUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            roles.ToList().ForEach(rol => claims.Add(new Claim(ClaimTypes.Role, rol)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tiempo de expiración del token. Una hora por ejemplo
            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return new LoginResponseViewModel()
            {
                Id = applicationUser.Id,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                //Expiration = expiration
            };
        }
    }
}
