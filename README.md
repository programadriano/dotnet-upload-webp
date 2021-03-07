# .NET Core Upload de imagens

Nesse repositório você tem um exemplo prático demonstrando como fazer upload de imagens utilizando o *.NET 5* e como converter essa imagem para **WebP**.

Mas o que seria esse formato **WebP** ?

*Criado em 2010 pelo Google, o formato WebP representa imagens com a mesma qualidade em um tamanho menor. Ou seja, economiza espaço, aumenta a velocidade de uma página e não perde em qualidade. Na prática, tem como principal objetivo compactar imagens de forma mais eficiente para oferecer uma experiência mais rápida ao usuário. fonte: https://rockcontent.com/br/blog/webp/#oque* 


# Fluxo do projeto

O projeto foi desenvolvido com uma `controller` e uma c


```Csharp
//Pacotes importados no projeto
Install-Package System.Drawing.Common
Install-Package ImageProcessor
Install-Package ImageProcessor.Plugins.WebP
```


```Csharp
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
```

## Testando o projeto

O projeto esta configurado com o swagger para facilitar o teste. Basta rodar ele e enviar uma imagem do seu desktop para validar o fluxo do método post.