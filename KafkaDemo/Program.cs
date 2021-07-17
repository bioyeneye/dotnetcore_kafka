using KafkaDemo.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace KafkaDemo
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }
        public static IServiceProvider ServiceProvider { get; set; }
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();

            var app = ServiceProvider.GetService<App>();
            app.InitConsole();
        }

        // use startup class without extensions
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostBuilderContext, serviceCollection) =>
                {
                    new Startup(hostBuilderContext.Configuration)
                    .ConfigureServices(serviceCollection);

                    ServiceProvider = serviceCollection.BuildServiceProvider();
                });

        // use startup class with extensions
        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //    .ConfigureAppConfiguration((hostingContext, config) =>
        //    {
        //        var env = hostingContext.HostingEnvironment;
        //        config
        //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
        //        Configuration = config.Build();
        //    })
        //    .UseStartup<Startup>();


        //use without startup
        /*private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    Configuration = config.Build();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddOptions();
                    services.Configure<KafkaConfig>(option =>
                    {
                        string brokerList = Configuration.GetValue<string>("Kafka:EH_FQDN");
                        string topic = Configuration.GetValue<string>("Kafka:PRODUCER_TOPIC");

                        option.ProducerTopic = topic;
                        option.BootstrapServers = brokerList;
                    });
                    services.AddHostedService<KafkaProducerHostedService>();
                    services.AddHostedService<KafkaConsumerHostedService>();
                });*/
    }
}
