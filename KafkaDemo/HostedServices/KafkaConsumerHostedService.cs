using Kafka.Public;
using Kafka.Public.Loggers;
using KafkaDemo.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaDemo.HostedServices
{
    public class KafkaConsumerHostedService : IHostedService
    {
        public ILogger<KafkaConsumerHostedService> _logger { get; }
        public IOptions<KafkaConfig> KafkaConfig { get; set; }

        public ClusterClient _cluster;

        public KafkaConsumerHostedService(ILogger<KafkaConsumerHostedService> logger, IOptions<KafkaConfig> kafkaConfig)
        {
            _logger = logger;
            KafkaConfig = kafkaConfig;
            _cluster = new ClusterClient(new Configuration
            {
                Seeds = KafkaConfig.Value.BootstrapServers,
            }, new ConsoleLogger());
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Consumer Started");
            _cluster.ConsumeFromLatest(topic: KafkaConfig.Value.ProducerTopic);
            _cluster.MessageReceived += record =>
             {
                 var des = Encoding.UTF8.GetString(record.Value as byte[]);
                 var objectt = JsonConvert.DeserializeObject<Transaction>(des);
                 _logger.LogInformation($"Received: {des}");
             };

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
