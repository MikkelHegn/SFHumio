using Newtonsoft.Json;
using Serilog;
using Serilog.Formatting.Compact;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ConsoleExample
{
    class Program
    {
        static int Main(string[] args)
        {
            var instanceId = Guid.NewGuid().ToString("N");
            var log = Path.Combine("log", $"ConsoleExample-{instanceId}.log"); // TODO: get from config.. env vars? 

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ProcessId", Process.GetCurrentProcess().Id.ToString())
                .Enrich.WithProperty("ServiceFabricInfo", GetServiceFabricInfo())
                .WriteTo.Console()
                .WriteTo.File(new RenderedCompactJsonFormatter(), log, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                Random random = new Random();

                while (true)
                {
                    int i = random.Next(100, 120);
                    Log.Information("Number of user sessions {@UserSessions}", i);

                    if (i == 120)
                    {
                        int ei = random.Next(0, 1000);
                        if (ei == 42)
                        {
                            int ooops = ei / 0;
                        }
                    }

                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }

            return 0;
        }

        private static string GetServiceFabricInfo()
        {
            ServiceFabricInfo sfInfo = new ServiceFabricInfo
            {
                NodeName = Environment.GetEnvironmentVariable("Fabric_NodeName"),
                ServiceName = Environment.GetEnvironmentVariable("Fabric_ServiceName"),
                ApplicationName = Environment.GetEnvironmentVariable("Fabric_ApplicationName"),
                ServicePackageActivationId = Environment.GetEnvironmentVariable("Fabric_ServicePackageActivationId")
            };

            return JsonConvert.SerializeObject(sfInfo); ;
        }

        private class ServiceFabricInfo
        {
            public string NodeName { get; set; }
            public string ServiceName { get; set; }
            public string ApplicationName { get; set; }
            public string ServicePackageActivationId { get; set; }
        }
    }
}
