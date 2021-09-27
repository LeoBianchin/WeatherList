using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace OpenWeather.API.Client
{
    public class WeatherClient
    {
        private readonly ServiceSettings settings;

        private readonly HttpClient httpClient;
        public WeatherClient(IOptions<ServiceSettings> settings, HttpClient httpClient)
        {
            this.settings = settings.Value;
            this.httpClient = httpClient;
        }

        public record Weather(string description, string main);
        public record Main(decimal temp);
        public record Forecast(Weather[] weather, Main main, long dt);

        public async Task<Forecast> GetCurrentWeatherAsync(string city)
        {
            var forecast = await httpClient.GetFromJsonAsync<Forecast>($"http://{settings.OpenWeatherHostUri}/data/2.5/weather?q={city}&appid={settings.ApiKey}&units=metric");
            return forecast;
        }
    }
}