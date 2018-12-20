using Newtonsoft.Json;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;
using Serilog.Events;
using Serilog.Parsing;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Serilog.Formatting;
using Serilog.Core;

namespace ConsoleExample
{
    class Foo{
        public int Bar { get; set; }        
        public int Baz { get; set; }        
    }

    class Program
    {
        static int Main(string[] args)
        {
            var instanceId = Guid.NewGuid().ToString("N");
            var log = Path.Combine("log", $"ConsoleExample-{instanceId}.log"); // TODO: get from config.. env vars? 

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.With(new ServiceFabricEnricher())       
                .WriteTo.Console(new KvFormatter())
               // .WriteTo.Console(new RenderedCompactJsonFormatter())
                //.WriteTo.File(new KvFormatter(), log, rollingInterval: RollingInterval.Day)
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
            if(!string.IsNullOrWhiteSpace(node))
                logEvent.AddPropertyIfAbsent(new LogEventProperty("Fabric_NodeName", new ScalarValue(node)));

            var service = Environment.GetEnvironmentVariable("Fabric_ServiceName");
            if(!string.IsNullOrWhiteSpace(service))
                logEvent.AddPropertyIfAbsent(new LogEventProperty("Fabric_ServiceName", new ScalarValue(service)));

            var application = Environment.GetEnvironmentVariable("Fabric_ApplicationName");
            if(!string.IsNullOrWhiteSpace(application))
                logEvent.AddPropertyIfAbsent(new LogEventProperty("Fabric_ApplicationName", new ScalarValue(application)));
        }
    }

    public class KvFormatter : ITextFormatter
    {
        public void Format(LogEvent logEvent, TextWriter output)
        {
            output.Write(logEvent.Timestamp.ToString("s", System.Globalization.CultureInfo.InvariantCulture));
            output.Write(" Level=");
            output.Write(logEvent.Level);
            output.Write(" ");
            output.Write(" ");
            output.Write(logEvent.RenderMessage());
            output.Write(". ");

            foreach(var p in logEvent.Properties)
            {
                output.Write(p.Key);
                output.Write("='");
                output.Write(p.Value);
                output.Write("' ");
            }

            if(logEvent.Exception != null)
            {
                output.Write("Exception='");
                output.Write(logEvent.Exception.ToString());
                output.Write("'");
                output.Write("StackTrace='");
                output.Write(logEvent.Exception.StackTrace);
                output.Write("'");
            }

            output.Write("\n");
        }
    }
}
