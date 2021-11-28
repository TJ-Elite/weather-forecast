using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherLibrary.Logic;
using WeatherLibrary.Models;

namespace WeatherLibrary.Api
{
    /// <summary>
    /// Contains methods which make API calls to openweathermap.org.
    /// </summary>
    internal static class OpenWeatherMapApi
    {
        private const int ResultLimit = 5;
        private const string Units = "metric";
        private const string Language = "fi";
        private const string Exclude = "current,minutely,hourly,alerts";

        /// <summary>
        /// Makes a geocoding API call based on the name specified by the user.
        /// </summary>
        /// <returns>A list of cities matching the search. The maximum number of results is five.</returns>
        internal static async Task SearchForMatchesAsync()
        {
            string url = $"geo/1.0/direct?q={WeatherLogic.UserInput}&limit={ResultLimit}&appid={WeatherLogic.ApiKey}";

            WeatherLogic.Cities = await MakeApiCallAsync<List<GeocodingModel>>(url);
        }

        /// <summary>
        /// Makes a weather forecast API call using the coordinates acquired from the geocoding call.
        /// </summary>
        /// <returns>Weather forecast data for the next seven days.</returns>
        internal static async Task GetWeatherForecastAsync()
        {
            string url = $"data/2.5/onecall?lat={WeatherLogic.Cities[WeatherLogic.Index].Latitude}&lon={WeatherLogic.Cities[WeatherLogic.Index].Longitude}&units={Units}&lang={Language}&exclude={Exclude}&appid={WeatherLogic.ApiKey}";

            WeatherLogic.Forecast = await MakeApiCallAsync<WeatherModel>(url);
        }

        /// <summary>
        /// Performs an API call to openweathermap.org.
        /// </summary>
        /// <typeparam name="T">Data model for the data you wish to extract from the response.</typeparam>
        /// <param name="url">URL containing all the specifics related to the API call, excluding the base part of the URL.</param>
        /// <returns>The data extracted from the JSON response based on the provided data model T.</returns>
        /// <exception cref="HttpRequestException"></exception>
        private static async Task<T> MakeApiCallAsync<T>(string url) where T : new()
        {
            var response = await ApiHelper.apiClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                if (jsonString != "[]")
                {
                    return JsonSerializer.Deserialize<T>(jsonString);
                }
                else
                {
                    WeatherLogic.UI.PrintToUser("Antamallesi nimelle ei löytynyt yhtään vastinetta.");
                }
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new HttpRequestException("API-avain ei ole kelvollinen!");
            }
            else if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                WeatherLogic.UI.PrintToUser("Tällä API-avaimella on tehty liikaa hakuja minuutin aikana.");
                WeatherLogic.UI.PrintToUser("Odota hetki ja kokeile sitten uudestaan.");
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                WeatherLogic.UI.PrintToUser("Antamallesi nimelle ei löytynyt yhtään vastinetta.");
            }
            else
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }
            return new T();
        }
    }
}
