using Confluent.Kafka;
using KafkaDemo.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaDemo.HostedServices
{
    public class KafkaProducerHostedService : IHostedService
    {
        private ILogger<KafkaProducerHostedService> _logger { get; }
        public IOptions<KafkaConfig> KafkaConfig { get; }

        private IProducer<Null, string> _producer;

        public KafkaProducerHostedService(ILogger<KafkaProducerHostedService> _logger, IOptions<KafkaConfig> kafkaConfig)
        {
            this._logger = _logger;
            KafkaConfig = kafkaConfig;
            var config = new ProducerConfig
            {
                BootstrapServers = KafkaConfig.Value.BootstrapServers
            };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            for (int i = 0; i < 100; i++)
            {
                var value = new Transaction { Name = $"Transaction {i}", Id = i+1};
                _logger.LogInformation(JsonConvert.SerializeObject(value));
                await _producer.ProduceAsync(KafkaConfig.Value.ProducerTopic, new Message<Null, string>()
                {
                    Value = JsonConvert.SerializeObject(value)
                }, cancellationToken);
            }

            _producer.Flush(TimeSpan.FromSeconds(10));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _producer?.Dispose();
            return Task.CompletedTask;
        }
    }
}
