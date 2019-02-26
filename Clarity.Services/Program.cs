namespace Clarity.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                await WebHost.CreateDefaultBuilder(args)
                    .ConfigureAppConfiguration(configBuilder => configBuilder.AddAzureKeyVault())
                    .ConfigureLogging(loggingBuilder => loggingBuilder.AddLogging())
                    .UseApplicationInsights()
                    .UseStartup<Startup>()
                    .Build()
                    .RunAsync(tokenSource.Token);
            }
        }
    }
}
