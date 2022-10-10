using CwkSocial.API.Controllers.Options;

namespace CwkSocial.API.Registrars
{
    public class SwaggerRegistrar : IWebApplicationBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen();

            builder.Services.ConfigureOptions<ConfigureSwaggerOpstions>();
        }
    }
}