using System.Text.Json.Serialization;

namespace WeatherLibrary.Models
{
    /// <summary>
    /// Contains temperature information for the daily weather forecast.
    /// </summary>
    public class TemperatureModel
    {
        /// <summary>
        /// The predicted minimum temperature for the day in Celsius.
        /// </summary>
        [JsonPropertyName("min")]
        public double Min { get; set; }

        /// <summary>
        /// The predicted maximum temperature for the day in Celsius.
        /// </summary>
        [JsonPropertyName("max")]
        public double Max { get; set; }
    }
}
