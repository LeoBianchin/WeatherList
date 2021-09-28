using System.Threading.Tasks;
using EventBus;
using MassTransit;
using Microsoft.Extensions.Logging;


namespace Consumer.API
{
    public class WeatherMessageConsumer : IConsumer<WeatherMessage>
    {
        ILogger<WeatherMessageConsumer> _logger;

        public WeatherMessageConsumer(ILogger<WeatherMessageConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<WeatherMessage> context)
        {
            return Task.Run(() => _logger.LogInformation($"City: {context.Message.City}, Temp: {context.Message.Temp} "));
        }
    }
}