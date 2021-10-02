using System.Threading.Tasks;
using EventBus;
using Microsoft.AspNetCore.SignalR;

namespace WeatherMessageHub
{
    public class WeatherHub : Hub<IWeatherHub>
    {
        public async Task SendMessage(WeatherMessage message)
        {
            await Clients.All.ReceiveMessage(message);
        }
    }
}