using System.Text.Json.Serialization;

namespace WeatherLibrary.Models
{
    /// <summary>
    /// Holds geocoding data for a city.
    /// </summary>
    public class GeocodingModel
    {
        /// <summary>
        /// The main name for the city.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Contains localized versions of the city's name in different languages.
        /// </summary>
        [JsonPropertyName("local_names")]
        public LocalNamesModel LocalNames { get; set; } = new LocalNamesModel();

        /// <summary>
        /// The city's latitude in degrees.
        /// </summary>
        [JsonPropertyName("lat")]
        public double Latitude { get; set; }

        /// <summary>
        /// The city's longitude in degrees.
        /// </summary>
        [JsonPropertyName("lon")]
        public double Longitude { get; set; }

        /// <summary>
        /// The country the city is in.
        /// </summary>
        [JsonPropertyName("country")]
        public string Country { get; set; }

        /// <summary>
        /// The state if the city is located in the US.
        /// </summary>
        [JsonPropertyName("state")]
        public string State { get; set; }
    }
}
