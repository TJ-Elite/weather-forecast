using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WeatherLibrary.Models
{
    /// <summary>
    /// Holds weather forecast data for one location.
    /// </summary>
    public class WeatherModel
    {
        /// <summary>
        /// Time zone offset from UTC in seconds.
        /// </summary>
        [JsonPropertyName("timezone_offset")]
        public long TimeZoneOffset { get; set; }

        /// <summary>
        /// List of daily weather forecast data.
        /// </summary>
        [JsonPropertyName("daily")]
        public List<DailyModel> Daily { get; set; } = new List<DailyModel>();
    }
}
