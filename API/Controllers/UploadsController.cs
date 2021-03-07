using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
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
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                    {
                        imageFactory.Load(image.OpenReadStream()) //carregando os dados da imagem
                                    .Format(new WebPFormat()) //formato
                                    .Quality(100) //parametro para não perder a qualidade no momento da compressão
                                    .Save(webPFileStream); //salvando a imagem
                    }
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
