using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenWeather.API.Dto;
using OpenWeather.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenWeather.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpenWeatherController : ControllerBase
    {
        private const string baseAddress = "http://api.openweathermap.org";
        private readonly IOptions<ConfigurationModel> _config;
        public OpenWeatherController(IOptions<ConfigurationModel> config)
        {
            _config = config;
        }

        [HttpGet("[action]/{city}")]
        public async Task<IActionResult> City(string city)
        {
            using var client = new HttpClient();

            try
            {
                client.BaseAddress = new Uri(baseAddress);
                var response = await client.GetAsync($"/data/2.5/weather?q={city}&appid={_config.Value.OpenWeatherAPIKey}&units=metric").ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var stringResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var rawWeather = JsonConvert.DeserializeObject<OpenWeatherResponse>(stringResult);
                return Ok(new
                {
                    rawWeather.Main.Temp,
                    Summary = string.Join(",", rawWeather.Weather.Select(x => x.Main)),
                    City = rawWeather.Name
                });
            }
            catch (HttpRequestException httpRequestException)
            {
                return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
            }
        }
    }
}