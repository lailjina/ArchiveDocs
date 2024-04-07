using ArchiveDocs.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace ArchiveDocs
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = config.GetConnectionString("PostgreSQLConnection");

            var hostBuilder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<ADDbContext>(options =>
                        options.UseNpgsql(connectionString));
                });

            var host = hostBuilder.Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    var dbContext = services.GetRequiredService<ADDbContext>();
                    dbContext.Database.Migrate(); // Применяем миграции при запуске приложения
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Обнаружены ошибки при миграции.");
                    Console.WriteLine(ex.Message);
                }
            }

            Application.Run(new Form1());
        }
    }
}
