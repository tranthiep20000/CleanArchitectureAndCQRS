namespace CwkSocial.API.Registrars
{
    public interface IWebApplicationRegistrar : IRegistrar
    {
        public void RegisterPipelineConponents(WebApplication app);
    }
}
