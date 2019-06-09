using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using AutoLagNotifier.Interfaces;
using AutoLagNotifier.Setting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace AutoLagNotifier
{
    class Program
    {
        private const string Appsettings = "appsettings.json";
        public static async Task Main(string[] args)
        {
            

            var host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.SetBasePath(Directory.GetCurrentDirectory());
                    configApp.AddJsonFile(Appsettings, optional: false);
                    configApp.AddJsonFile(
                        $"appsettings.json",
                        optional: false);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ILagChecker, LagChecker>();
                    services.AddHttpClient<ILagChecker,LagChecker>( 
                        o => o.BaseAddress = new Uri(hostContext.Configuration.GetSection("BurrowEndpoints")["BaseUrl"]));
                    
                    services.AddSingleton<INotifier, Notifier>();
                    services.AddLogging();
                    services.Configure<Config>(hostContext.Configuration.GetSection("RefreshTime"));
                    services.Configure<BurrowEndpoints>(hostContext.Configuration.GetSection("BurrowEndpoints"));
                    services.Configure<Lag>(hostContext.Configuration.GetSection("Lag"));
                    services.Configure<FilterName>(hostContext.Configuration.GetSection("Filter"));
                    services.AddHostedService<CheckService>();
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConfiguration(hostContext.Configuration).AddSerilog();
                })
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                    .ReadFrom
                    .Configuration(hostingContext.Configuration))
                .Build();
           
            

            await host.StartAsync();
        }
    }
}