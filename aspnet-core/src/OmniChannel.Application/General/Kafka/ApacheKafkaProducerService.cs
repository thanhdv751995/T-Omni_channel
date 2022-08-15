using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp;

namespace OmniChannel.General.Kafka
{
    [RemoteService(true)]
    public class ApacheKafkaProducerService : OmniChannelAppService
    {
        private readonly IConfiguration _configuration;
        public ApacheKafkaProducerService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

       [HttpPost]
        public async Task<bool>
        Post([FromBody] string messageTest)
        {
            string message = JsonSerializer.Serialize(messageTest);
            return await SendOrderRequest(_configuration.GetSection("Kafka")["Topic-Notification"], message);
        }
        private async Task<bool> SendOrderRequest(string topic, string message)
        {
            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = _configuration.GetSection("Kafka")["BootstrapServer"],
                ClientId = Dns.GetHostName()
            };

            try
            {
                using (var producer = new ProducerBuilder<Null, string>(config).Build())
                {
                    var result = await producer.ProduceAsync
                    (topic, new Message<Null, string>
                    {
                        Value = message
                    });
                    Debug.WriteLine($"Delivery Timestamp:{result.Timestamp.UtcDateTime}");
                    return await Task.FromResult(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured: {ex.Message}");
            }

            return await Task.FromResult(false);
        }
    }
}
