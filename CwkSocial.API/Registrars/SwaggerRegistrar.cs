using CwkSocial.API.Controllers.Options;
using Microsoft.AspNetCore.Http.Features;

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
