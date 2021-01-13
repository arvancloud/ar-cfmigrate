using ArvancloudLib.Api.Token;
using AutoMapper;
using CfMigrate.Api.Config.Mapper;
using CloudFlareLib.Api.Auth;
using CloudFlareLib.Api.Dns;
using CloudFlareLib.Api.DnsSec;
using CloudFlareLib.Api.Token;
using CloudFlareLib.Api.Zone;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CfMigrate.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });

            var mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);

            services.AddControllers();

            services.AddTransient<ICloudflareDnsService, CloudflareDnsService>();
            services.AddTransient<ICloudflareAuthService, CloudflareAuthService>();
            services.AddTransient<ICloudflareZoneService, CloudflareZoneService>();
            services.AddTransient<ICloudflareDnsSecService, CloudflareDnsSecService>();
            services.AddTransient<ICloudflareTokenService, CloudflareTokenService>();
            
            services.AddTransient<IArvancloudTokenService, ArvancloudTokenService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "CfMigrate.Api", Version = "v1"});
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CfMigrate.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}