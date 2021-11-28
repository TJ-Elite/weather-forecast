using System.Text.Json.Serialization;

namespace WeatherLibrary.Models
{
    /// <summary>
    /// A single day's weather forecast data for one location.
    /// </summary>
    public class DailyModel
    {
        /// <summary>
        /// UTC Unix time for the day the forecast is for.
        /// </summary>
        [JsonPropertyName("dt")]
        public long UnixTimeUtc { get; set; }

        /// <summary>
        /// Contains temperature information for the daily weather forecast.
        /// </summary>
        [JsonPropertyName("temp")]
        public TemperatureModel Temperature { get; set; } = new TemperatureModel();
    }
}
