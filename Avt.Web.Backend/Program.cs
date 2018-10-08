using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avt.Web.Backend.Helper;
using Avt.Web.Backend.Service;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Avt.Web.Backend
{
    public class Program
    {
        public static LongRunningWorkerService WorkerService;
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
                    logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug);
                })
                
                .UseStartup<Startup>().UseUrls("https://localhost:5005/");
    }
}
