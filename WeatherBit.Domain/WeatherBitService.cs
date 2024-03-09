using WeatherBit.Domain.Interfaces;

namespace WeatherBit.Domain
{
    public class WeatherBitService : IWeatherBitService
    {
        public string Url { get; set; } = String.Empty;
        public string Key { get; set; } = String.Empty;
    }
}
