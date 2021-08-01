using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Apply migration
            // A am using In Memory DB so i am commenting this code -  migration works only for Relational Database
           // ApplyMigrations(host.Services).Wait();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.UseStartup<Startup>());
        private static async Task ApplyMigrations(IServiceProvider serviceProvider)
        {
            using var serviceScope = serviceProvider.CreateScope();
            var services = serviceScope.ServiceProvider;
            var env = services.GetRequiredService<IWebHostEnvironment>();
           // if (!(env.IsStaging() || env.IsProduction())) return;   apply migration in if enviroment is not development       
            var logger = services.GetService<ILogger<BaseContext>>();
            logger.LogInformation($@"------------[ Applying Migration on Environment: '{env.EnvironmentName}' ]-----------");
            var context = services.GetRequiredService<BaseContext>();
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
                await context.Database.MigrateAsync();
        }
    }
}
