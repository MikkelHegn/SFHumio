using Serilog;
using Serilog.Core;
using Serilog.Events;
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
                .Enrich.With(new ServiceFabricEnricher())
                .WriteTo.File(new RenderedCompactJsonFormatter(), log, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                var random = new Random();

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
    }


    public class ServiceFabricEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var node = Environment.GetEnvironmentVariable("Fabric_NodeName");
            if (!string.IsNullOrWhiteSpace(node))
            {
                logEvent.AddPropertyIfAbsent(new LogEventProperty("Fabric_NodeName", new ScalarValue(node)));
            }

            var service = Environment.GetEnvironmentVariable("Fabric_ServiceName");
            if (!string.IsNullOrWhiteSpace(service))
            {
                logEvent.AddPropertyIfAbsent(new LogEventProperty("Fabric_ServiceName", new ScalarValue(service)));
            }

            var application = Environment.GetEnvironmentVariable("Fabric_ApplicationName");
            if (!string.IsNullOrWhiteSpace(application))
            {
                logEvent.AddPropertyIfAbsent(new LogEventProperty("Fabric_ApplicationName", new ScalarValue(application)));
            }

            var processId = Process.GetCurrentProcess().Id.ToString();
            if (!string.IsNullOrWhiteSpace(processId))
            {
                logEvent.AddPropertyIfAbsent(new LogEventProperty("ProcessId", new ScalarValue(processId)));
            }
        }
    }

}
