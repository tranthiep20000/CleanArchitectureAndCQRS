using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace CwkSocial.API.Registrars
{
    public class SwaggerWebApplicationRegistrar : IWebApplicationRegistrar
    {
        public void RegisterPipelineConponents(WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.ApiVersion.ToString());
                }
            });
        }
    }
}
