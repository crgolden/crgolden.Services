namespace Clarity.Services
{
    using System.IO;
    using System.Threading.Tasks;
    using Core;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            await new HostBuilder()
                .ConfigureHostConfiguration(configBuilder =>
                {
                    configBuilder.SetBasePath(Directory.GetCurrentDirectory());
                    configBuilder.AddJsonFile(
                        path: "hostsettings.json",
                        optional: true);
                    configBuilder.AddEnvironmentVariables();
                    configBuilder.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((context, configBuilder) =>
                {
                    configBuilder.AddJsonFile(
                        path: "appsettings.json",
                        optional: true);
                    configBuilder.AddJsonFile(
                        path: $"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
                        optional: true);
                    configBuilder.AddEnvironmentVariables();
                    configBuilder.AddAzureKeyVault();
                    configBuilder.AddCommandLine(args);
                })
                .ConfigureServices((context, services) =>
                {
                    services.Configure<EmailOptions>(context.Configuration.GetSection(nameof(EmailOptions)));
                    services.Configure<StorageOptions>(context.Configuration.GetSection(nameof(StorageOptions)));
                    services.Configure<ServiceBusOptions>(context.Configuration.GetSection(nameof(ServiceBusOptions)));
                    services.AddSingleton<IEmailService, SendGridEmailService>();
                    services.AddSingleton<IStorageService, AzureBlobStorageService>();
                    services.AddHostedService<EmailQueueClient>();
                })
                .ConfigureLogging(loggingBuilder =>
                {
                    loggingBuilder.AddConsole();
                    loggingBuilder.AddLogging();
                })
                .UseConsoleLifetime()
                .Build()
                .RunAsync();
        }
    }
}
