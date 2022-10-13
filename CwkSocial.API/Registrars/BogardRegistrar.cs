using CwkSocial.APPLICATION.UserProfiles.Queries;
using MediatR;

namespace CwkSocial.API.Registrars
{
    public class BogardRegistrar : IWebApplicationBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(Program), typeof(GetAllUserProfileQuery));
            builder.Services.AddMediatR(typeof(GetAllUserProfileQuery));
        }
    }
}
