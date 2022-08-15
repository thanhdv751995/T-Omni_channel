using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OmniChannel.Kafka
{
    public class ApacheKafkaConsumerService : BackgroundService
    {
        //private readonly string topic = "test";
        //private readonly string groupId = "test_group";
        //private readonly string bootstrapServers = "localhost:9092";
        private readonly ConsumerConfig conf;
        private readonly IConfiguration _configuration;
        public ApacheKafkaConsumerService(IConfiguration configuration)
        {
            _configuration = configuration; 
            conf = new ConsumerConfig
            {
                GroupId = _configuration.GetSection("Kafka")["GroupId"],
                BootstrapServers = _configuration.GetSection("Kafka")["BootstrapServer"],
                AutoOffsetReset = AutoOffsetReset.Earliest,
                MaxPartitionFetchBytes = 734003
            };
        }
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public Task StopAsync(CancellationToken cancellationToken)
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        {
            return Task.CompletedTask;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var task = new Task(() => HandleKafkaEvent(stoppingToken));
            task.Start();

            return Task.CompletedTask;
        }

        private void HandleKafkaEvent(CancellationToken stoppingToken)
        {
            try
            {
                using var consumerBuilder = new ConsumerBuilder<Ignore, string>(conf).Build();

                consumerBuilder.Subscribe(_configuration.GetSection("Kafka")["Topic-Notification"]);
                var cancelToken = new CancellationTokenSource();

                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cancelToken.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var consumer = consumerBuilder.Consume(cancelToken.Token);

                            var orderRequest = JsonSerializer.Deserialize<string>(consumer.Message.Value);

                            Debug.WriteLine($"Processing name Product: {orderRequest}");
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }

                    }
                }
                catch (OperationCanceledException)
                {
                    consumerBuilder.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
