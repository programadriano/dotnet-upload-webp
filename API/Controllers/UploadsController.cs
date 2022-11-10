
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using SkiaSharp;
using System.IO;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadsController : ControllerBase
    {

        [HttpPost]
        public IActionResult Index(IFormFile image)
        {
            try
            {
                //validação simples, caso não tenha sido enviado nenhuma imagem para upload nós estamos retornando null
                if (image == null) return null;

                //Salvando a imagem no formato enviado pelo usuário
                using (var stream = new FileStream(Path.Combine("Imagens", image.FileName), FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                // Salvando no formato WebP
                using (var webPFileStream = new FileStream(Path.Combine("Imagens", Guid.NewGuid() + ".webp"), FileMode.Create))
                {

                    SKBitmap.Decode(image.OpenReadStream()).Encode(SKEncodedImageFormat.Webp, 80).SaveTo(webPFileStream);


                }

                return Ok("Imagem salva com sucesso!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro no upload: " + ex.Message);
            }

        }
    }
}
