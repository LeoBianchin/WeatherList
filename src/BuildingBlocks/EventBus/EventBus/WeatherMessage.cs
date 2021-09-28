using System;

namespace EventBus
{
    public class WeatherMessage
    {
        public string City { get; set; }
        
        public decimal Temp { get; set; }
    }
}