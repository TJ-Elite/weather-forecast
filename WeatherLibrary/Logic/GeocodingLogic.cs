using System;
using System.Threading.Tasks;
using WeatherLibrary.Api;
using WeatherLibrary.Models;

namespace WeatherLibrary.Logic
{
    /// <summary>
    /// Contains methods related to asking the user for the name of a city and acquiring geographic coordinates for it.
    /// </summary>
    public static class GeocodingLogic
    {
        /// <summary>
        /// Constructs a line of text containing a city's information.
        /// <example>
        /// An example of the output would be:
        /// 1. Oulu, FI, 65°1'N, 25°28'E
        /// </example>
        /// </summary>
        /// <param name="city">Data model containing geocoding data for a city.</param>
        /// <param name="index">Index matching the city in the list of cities where the city's data resides.</param>
        /// <returns>The city's data formatted to a single line to be printed to the user.</returns>
        public static string ConstructPhrase(GeocodingModel city, int index)
        {
            string phrase = $"{index + 1}. {city.Name}";
            if (city.LocalNames.Finnish != city.Name && !string.IsNullOrWhiteSpace(city.LocalNames.Finnish))
            {
                phrase += $" ({city.LocalNames.Finnish})";
            }
            else if (city.LocalNames.English != city.Name && !string.IsNullOrWhiteSpace(city.LocalNames.English))
            {
                phrase += $" ({city.LocalNames.English})";
            }
            phrase += $", {city.Country}";
            if (!string.IsNullOrWhiteSpace(city.State))
            {
                phrase += $" ({city.State})";
            }

            // Convert the decimal parts of latitude and longitude to arcminutes.
            uint latitudeArcminute = city.Latitude.GetArcminutePart();
            uint longitudeArcminute = city.Longitude.GetArcminutePart();

            phrase += $", {Math.Abs(Math.Truncate(city.Latitude))}°{latitudeArcminute}'";
            if (city.Latitude >= 0.0)
            {
                phrase += "N";
            }
            else
            {
                phrase += "S";
            }
            phrase += $", {Math.Abs(Math.Truncate(city.Longitude))}°{longitudeArcminute}'";
            if (city.Longitude >= 0.0)
            {
                phrase += "E";
            }
            else
            {
                phrase += "W";
            }

            return phrase;
        }

        /// <summary>
        /// Converts the fractional part of the given value in degrees to an unsigned integer representing arcminutes.
        /// </summary>
        /// <param name="degrees">Latitude or longitude in degrees.</param>
        /// <returns>Arcminutes as an unsigned integer.</returns>
        public static uint GetArcminutePart(this double degrees)
        {
            double arcminutePart = Math.Abs(degrees - Math.Truncate(degrees)) * 60.0;
            return (uint)Math.Round(arcminutePart, 0, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Top-level method for asking the user for the name of a city, if necessary, and performing a geocoding API call.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        internal static async Task GetCoordinatesAsync()
        {
            await GetCitiesAsync();

            WeatherLogic.Index = 0;
            // If the search returns more than one location matching the name, ask the user to specify which one is the one they mean.
            if (WeatherLogic.Cities.Count > 1)
            {
                AskForIndex();
            }
        }

        /// <summary>
        /// Asks the user for the name of a city if one hasn't been provided already and tries to get matching geocoding data for it.
        /// If no matches are found, the user is asked for a new name until a valid result is found.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        private static async Task GetCitiesAsync()
        {
            // Clear the list of search results.
            WeatherLogic.Cities.Clear();

            while (WeatherLogic.Cities.Count == 0)
            {
                if (string.IsNullOrWhiteSpace(WeatherLogic.UserInput))
                {
                    AskForCity();
                }

                await OpenWeatherMapApi.SearchForMatchesAsync();

                if (WeatherLogic.Cities.Count == 0)
                {
                    // Set user input string to empty so that we ask for it again.
                    WeatherLogic.UserInput = "";
                }
            }
        }

        /// <summary>
        /// Keeps asking the user for the name of a city until they provide an answer.
        /// </summary>
        private static void AskForCity()
        {
            WeatherLogic.UI.PrintToUser("Syötä kaupunki, jonka sääennusteen haluat: ", false);
            WeatherLogic.UserInput = WeatherLogic.UI.GetUserInput();
            while (string.IsNullOrWhiteSpace(WeatherLogic.UserInput))
            {
                WeatherLogic.UI.PrintToUser("Et syöttänyt mitään. Syötä kaupungin nimi: ", false);
                WeatherLogic.UserInput = WeatherLogic.UI.GetUserInput();
            }
            // Replace whitespaces with "+".
            WeatherLogic.UserInput = WeatherLogic.UserInput.Replace(' ', '+');
        }

        /// <summary>
        /// Keeps asking the user for an index matching one of the geocoding search results until they provide a valid response.
        /// The displayed indexes have been shifted up to start from one.
        /// </summary>
        private static void AskForIndex()
        {
            PrintCities();

            int index;
            do
            {
                WeatherLogic.UI.PrintToUser("Syötä tarkoittamasi kaupungin numero: ", false);
                if (!int.TryParse(WeatherLogic.UI.GetUserInput(), out index))
                {
                    WeatherLogic.UI.PrintToUser("Et syöttänyt numeroa!");
                }
                else if (index < 1 || index > WeatherLogic.Cities.Count)
                {
                    WeatherLogic.UI.PrintToUser($"Antamasi numeron täytyy olla välillä 1–{WeatherLogic.Cities.Count}!");
                }
            } while (index < 1 || index > WeatherLogic.Cities.Count);

            // Shift index to zero-based indexing.
            WeatherLogic.Index = --index;
        }

        /// <summary>
        /// Prints a list of the geocoding search results to the user.
        /// </summary>
        private static void PrintCities()
        {
            WeatherLogic.UI.PrintToUser("Antamallasi nimellä löytyivät seuraavat tulokset");
            for (int i = 0; i < WeatherLogic.Cities.Count; i++)
            {
                WeatherLogic.UI.PrintToUser(ConstructPhrase(WeatherLogic.Cities[i], i));
            }
        }
    }
}
