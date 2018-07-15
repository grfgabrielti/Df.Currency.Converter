using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConversorDeMoedas.ACL.Factory;
using ConversorDeMoedas.ACL.Interface.Factory;
using ConversorDeMoedas.Domain.Factory;
using ConversorDeMoedas.Domain.Interface.Factory;
using ConversorDeMoedas.Services;
using ConversorDeMoedas.Services.Factory;
using ConversorDeMoedas.Services.Interface;
using ConversorDeMoedas.Services.Interface.Factory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ConversorDeMoedas.API.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddTransient<IConversorServiceFactory, ConversorServiceFactory>();
            services.AddTransient<IConversorService, ConversorService>();
            services.AddTransient<IConversorACLFactory, ConversorACLFactory>();
            services.AddTransient<IMoedaFactory, MoedaFactory>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
