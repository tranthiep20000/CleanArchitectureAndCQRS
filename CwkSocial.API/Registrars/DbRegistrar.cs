﻿using CwkSocial.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.API.Registrars
{
    public class DbRegistrar : IWebApplicationBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            var cs = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(cs));
        }
    }
}
