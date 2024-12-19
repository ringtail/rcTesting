//using Microsoft.ServiceFabric.Services.Runtime;
//using System;
//using System.Diagnostics;
//using System.Threading;
//using System.Threading.Tasks;
//
//namespace rcTesting.api
//{
//    internal static class Program
//    {
//        /// <summary>
//        /// This is the entry point of the service host process.
//        /// </summary>
//        private static void Main()
//        {
//            try
//            {
//                // The ServiceManifest.XML file defines one or more service type names.
//                // Registering a service maps a service type name to a .NET type.
//                // When Service Fabric creates an instance of this service type,
//                // an instance of the class is created in this host process.
//
//                ServiceRuntime.RegisterServiceAsync("rcTesting.apiType",
//                    context => new api(context)).GetAwaiter().GetResult();
//
//                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(api).Name);
//
//                // Prevents this host process from terminating so services keeps running.
//                Thread.Sleep(Timeout.Infinite);
//            }
//            catch (Exception e)
//            {
//                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
//                throw;
//            }
//        }
//    }
//}

// 將Service Fabric的註冊依賴進行剝離，使用默認.net的webServer進行替代和管理。

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace rcTesting.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}

