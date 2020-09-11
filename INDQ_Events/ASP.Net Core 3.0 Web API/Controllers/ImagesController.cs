using ASP.Net_Core_3._0_Web_API.Infraestructure.Data;
using ASP.Net_Core_3._0_Web_API.ViewModels.Image;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ASP.Net_Core_3._0_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ImagesController : IndqControllerBase
    {
        private readonly ImagesRepository _repository;
        public static IWebHostEnvironment _environment;

        public ImagesController(ImagesRepository repository, IWebHostEnvironment environment)
        {
            _repository = repository;
            _environment = environment;
        }

        [HttpPost]
        [Route("/images")]
        public async Task<ActionResult<ImageResponseViewModel>> Post(IFormFile file)
        {
            try
            {
                string webRootPath = _environment.WebRootPath + "\\Upload\\";
                if (!Directory.Exists(webRootPath))
                {
                    Directory.CreateDirectory(webRootPath);
                }

                string id = Guid.NewGuid().ToString();
                string fileName = $"{Guid.NewGuid().ToString()}{ Path.GetExtension(file.FileName)}";

                using (FileStream fs =
                    System.IO.File.Create($"{webRootPath}{fileName}"))
                {
                    await file.CopyToAsync(fs);
                    await fs.FlushAsync();
                }

                return Ok(new ImageResponseViewModel() { Id = id, FileName = fileName });
            }
            catch (Exception ex)
            {
                LogException(ex);
                return StatusCode(400, $"Error al subir imagen");
            }
        }


        [HttpGet]
        [Route("/images/{fileName}")]
        [AllowAnonymous]
        public async Task<ActionResult> Get(string fileName)
        {
            try
            {
                string filePath = $"{_environment.WebRootPath}\\Upload\\{fileName}";

                if (!System.IO.File.Exists(filePath))
                    return NotFound("Imagen no encontrada");

                Byte[] bytes = await System.IO.File.ReadAllBytesAsync(filePath);

                return File(bytes, GetContentType(filePath));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        private string GetContentType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
    }

}
