using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;

namespace ConsoleExample
{
    class Program
    {
        static int Main(string[] args)
        {
          //  var instanceId = Guid.NewGuid().ToString("N");
            var instanceId = "";
            var log = Path.Combine("ApplicationLogs", $"ConsoleExample-{instanceId}.txt"); // TODO: get from config.. env vars? 

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(new RenderedCompactJsonFormatter(),log, rollingInterval: RollingInterval.Day)
                .CreateLogger();


            try
            {
                while(true)
                {
                    Log.Information("Printing out {@Number}", x);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }
    }
}
