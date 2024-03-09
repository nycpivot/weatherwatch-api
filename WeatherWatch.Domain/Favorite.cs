using System.ComponentModel.DataAnnotations;

namespace WeatherWatch.Domain
{
    public class Favorite
    {
        [Key]
        public string ZipCode { get; set; } = String.Empty;
    }
}
