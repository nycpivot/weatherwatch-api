using WeatherWatch.Api.Interfaces;

namespace WeatherWatch.Api
{
    public class WeatherDataService : IWeatherDataService
    {
        public string Url { get; set; } = String.Empty;
    }
}
