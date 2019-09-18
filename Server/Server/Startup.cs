using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rubius.DevSaunaB.Server.Data;
using Rubius.DevSaunaB.Server.Models;
using Rubius.DevSaunaB.Server.Services;

namespace Rubius.DevSaunaB.Server
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddCors();

            services.AddSingleton<ObjectsCreatorService>();
            services.AddSingleton<ClientConnection>();
            services.AddSingleton<ObjectsStatesCollection>();
            services.AddTransient<IConnectionHandler, ConnectionHandler>();
            services.AddTransient<IDataObjectService, DataObjectService>();
            services.AddTransient<IBindingService, BindingService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ObjectsCreatorService creator)
        {
            loggerFactory.AddConsole();
            app.UseCors(builder => builder.AllowAnyOrigin());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
