using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace CwkSocial.API.Registrars
{
    public class MvcWebApplicationRegistrar : IWebApplicationRegistrar
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

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
        }
    }
}