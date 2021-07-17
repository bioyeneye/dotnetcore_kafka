using KafkaDemo.HostedServices;
using KafkaDemo.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KafkaDemo
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<KafkaConfig>(option =>
            {
                string brokerList = Configuration.GetValue<string>("Kafka:EH_FQDN");
                string topic = Configuration.GetValue<string>("Kafka:PRODUCER_TOPIC");

                option.ProducerTopic = topic;
                option.BootstrapServers = brokerList;
            });
            services.AddHostedService<KafkaConsumerHostedService>();
            services.AddHostedService<KafkaProducerHostedService>();
            services.AddSingleton<App>();
        }
    }
}