using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OmniChannel.Kafka;
using OmniChannel.TiktokShop.Signal;
using Serilog;
using Serilog.Events;

namespace OmniChannel;

public class Program
{
    public async static Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Async(c => c.File("Logs/logs.txt"))
            .WriteTo.Async(c => c.Console())
            .CreateLogger();

        try
        {
            Log.Information("Starting OmniChannel.HttpApi.Host.");
            var builder = WebApplication.CreateBuilder(args);

            await builder.AddApplicationAsync<OmniChannelHttpApiHostModule>();
            builder.Services.AddSignalR();
            builder.Host.AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseSerilog();
            //builder.Host.ConfigureServices((context, collection) =>
            //{
            //    collection.AddHostedService<ApacheKafkaConsumerService>();
            //});
            var app = builder.Build();
            app.MapHub<SignalR>("/notify");
            await app.InitializeApplicationAsync();
            await app.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly!");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
