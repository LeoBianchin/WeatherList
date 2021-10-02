using System;
using System.Threading.Tasks;
using EventBus;

namespace WeatherMessageHub
{
    public interface IWeatherHub
    {
        Task ReceiveMessage(WeatherMessage message);
    }
}