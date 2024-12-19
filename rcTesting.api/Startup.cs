using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
// 增加引入路由相關的包
using Microsoft.AspNetCore.Routing;

namespace rcTesting.api
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
            // Add framework services.
            services.AddMvc()
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    });

            // The following line enables Application Insights telemetry collection.
            // https://docs.microsoft.com/zh-tw/azure/azure-monitor/app/sampling
            var aiOptions = new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions();
            aiOptions.EnableAdaptiveSampling = false;
            services.AddApplicationInsightsTelemetry(aiOptions);
            
            // Adds services required for using options.
            services.AddOptions();

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml" });
            });

            //Set Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "rcTesting API",
                    Description = "Ricacorp Service WebApi",
                    Contact = new OpenApiContact { Name = "Ching Wong", Email = "chingwong@ricacorp.com" },
                });

                //Set the comments path for the swagger json and ui.
                //var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "rcPostV3.api.xml");
                var filePath = Path.Combine(Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath, "rcTesting.api.xml");
                c.IncludeXmlComments(filePath);
            });

            services.AddHttpClient(Options.DefaultName)
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Gzip
            app.UseResponseCompression();

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "rcTesting");
            });

            app.UseStaticFiles();
            // app.UseMvc();
            // 將註冊路由的動作從Service Fabric的註冊，轉為.net內置的註冊。
            app.UseMvc(routes =>
            {
              routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
