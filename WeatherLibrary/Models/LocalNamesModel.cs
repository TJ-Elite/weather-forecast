using System.Text.Json.Serialization;

namespace WeatherLibrary.Models
{
    /// <summary>
    /// Contains localized versions of the city's name in different languages.
    /// </summary>
    public class LocalNamesModel
    {
        /// <summary>
        /// The city's name in English.
        /// </summary>
        [JsonPropertyName("en")]
        public string English { get; set; }

        /// <summary>
        /// The city's name in Finnish.
        /// </summary>
        [JsonPropertyName("fi")]
        public string Finnish { get; set; }
    }
}
