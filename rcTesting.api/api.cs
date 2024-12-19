//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
//using Microsoft.ServiceFabric.Services.Communication.Runtime;
//using Microsoft.ServiceFabric.Services.Runtime;
//using System.Collections.Generic;
//using System.Fabric;
//using System.Fabric.Health;
//using System.IO;
//
//namespace rcTesting.api
//{
//    /// <summary>
//    /// The FabricRuntime creates an instance of this class for each service type instance.
//    /// </summary>
//    internal sealed class api : StatelessService
//    {
//        public api(StatelessServiceContext context)
//            : base(context)
//        { }
//
//        /// <summary>
//        /// Optional override to create listeners (like tcp, http) for this service instance.
//        /// </summary>
//        /// <returns>The collection of listeners.</returns>
//        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
//        {
//#if DEBUG
//            HealthInformation healthInformation = new HealthInformation("ProjectConfiguration", "Debug", HealthState.Error);
//            this.Partition.ReportInstanceHealth(healthInformation);
//#endif
//
//            return new ServiceInstanceListener[]
//            {
//                new ServiceInstanceListener(serviceContext =>
//                    new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
//                    {
//                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");
//
//                        return new WebHostBuilder()
//                                    .UseKestrel()
//                                    .ConfigureServices(
//                                        services => services
//                                            .AddSingleton<StatelessServiceContext>(serviceContext))
//                                    .ConfigureAppConfiguration((hostingContext, config) =>
//                                    {
//                                        var env = hostingContext.HostingEnvironment;
//
//                                        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//                                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
//                                            .AddEnvironmentVariables()
//                                            .SetBasePath(env.ContentRootPath);
//                                    })
//                                    .ConfigureLogging((hostingContext, logging)  => {
//                                        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
//                                        // add ETW logging https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?tabs=aspnetcore2x#eventsource
//                                        // this generate a lot of "The parameters to the Event method do not match the parameters to the WriteEvent method. This may cause the event to be displayed incorrectly." in debug window
//                                        // and no events in Diagnostic Window of Visual Studio, but events itself are recorded (tested with PSH event etl file)
//                                        // see more https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/608, https://stackoverflow.com/questions/42123222/tpl-etw-events-have-extra-parameters-that-cause-excessive-debugger-output
//                                        logging.AddEventSourceLogger();
//
//                                    })
//                                    .UseContentRoot(Directory.GetCurrentDirectory())
//                                    .UseStartup<Startup>()
//                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
//                                    .UseUrls(url)
//                                    .Build();
//                    }))
//            };
//        }
//    }
//}



// 這個文件已經不需要了，裡面主要是通過Service Fabric的Stateless進行實例化的過程，這個邏輯已經都放在了Startup.cs中。