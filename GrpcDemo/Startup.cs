using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GrpcDemo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    var endpointDataSource = context
                        .RequestServices.GetRequiredService<EndpointDataSource>();
                    await context.Response.WriteAsJsonAsync(new
                    {
                        results = endpointDataSource
                            .Endpoints
                            .OfType<RouteEndpoint>()
                            .Where(e => e.DisplayName?.StartsWith("gRPC") == true)
                            .Select(e => new
                            {
                                name = e.DisplayName, 
                                pattern = e.RoutePattern.RawText,
                                order = e.Order
                            })
                            .ToList()
                    });
                });
                
                endpoints.MapGrpcService<Service>();
            });
        }
    }
}