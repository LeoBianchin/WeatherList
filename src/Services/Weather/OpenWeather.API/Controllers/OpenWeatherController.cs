using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenWeather.API.Client;
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
        private readonly WeatherClient client;
        public OpenWeatherController(WeatherClient client)
        {
            this.client = client;
        }

        [HttpGet("[action]/{city}")]
        public async Task<IActionResult> Forecast(string city)
        {
            try
            {
                var forecast = await client.GetCurrentWeatherAsync(city);

                return Ok(forecast);
            }
            catch (HttpRequestException httpRequestException)
            {
                return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
            }
        }
    }
}