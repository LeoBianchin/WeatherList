using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Playlist.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        static bool? _isRunningInContainer;

        static bool IsRunningInContainer =>
            _isRunningInContainer ??= bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inContainer) && inContainer;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<WeatherMessageConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbithost = "localhost";

                    if (IsRunningInContainer)
                        rabbithost = "rabbitmq";

                    cfg.Host(rabbithost, "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ReceiveEndpoint("event-listener", e => e.ConfigureConsumer<WeatherMessageConsumer>(context));
                });
            });

            services.AddMassTransitHostedService();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Playlist.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Playlist.API v1"));
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
