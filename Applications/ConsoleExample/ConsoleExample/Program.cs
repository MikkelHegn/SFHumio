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
            var instanceId = Guid.NewGuid().ToString("N");
            var log = Path.Combine("log", $"ConsoleExample-{instanceId}.txt"); // TODO: get from config.. env vars? 

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(new RenderedCompactJsonFormatter(),log, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    Log.Information("Printing out {@Number}", i);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Terminated unexpectedly");
                }
            finally
            {
                Log.CloseAndFlush();
            }

            return 0;
        }
    }
}
