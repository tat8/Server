using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rubius.DevSaunaB.DataServer.Extensions;
using Rubius.DevSaunaB.DataServer.Handlers;
using Rubius.DevSaunaB.DataServer.Models;
using Rubius.DevSaunaB.DataServer.Services;

namespace Rubius.DevSaunaB.DataServer
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
            services.AddWebSocketManager();

            services.AddSingleton<SocketStateHolder>();
            services.AddSingleton<StoreCollection>();
            services.AddTransient<IBindingService, BindingService>();
            services.AddTransient<IDataObjectService, DataObjectService>();
            services.AddTransient<IRequestHandler, RequestHandler>();
            services.AddTransient<IStoreService, StoreService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseStaticFiles();
            app.UseWebSockets();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.MapWebSocketManager("/messages", serviceProvider.GetService<MessageHandler>());
        }
    }
}
