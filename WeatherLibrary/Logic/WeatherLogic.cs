using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WeatherLibrary.Api;
using WeatherLibrary.Models;

namespace WeatherLibrary.Logic
{
    /// <summary>
    /// Contains the library's top-level logic for requesting weather forecast data.
    /// </summary>
    public static class WeatherLogic
    {
        private static IUserInterface _ui;
        private static string _apiKey;
        private static string _userInput;

        internal static IUserInterface UI
        {
            get => _ui;
            private set => _ui = value ?? throw new ArgumentNullException(nameof(value), $"{nameof(UI)} cannot be null!");
        }

        internal static string ApiKey
        {
            get => _apiKey;
            private set => _apiKey = value;
        }

        internal static string UserInput
        {
            get => _userInput;
            set => _userInput = value.Trim();
        }

        internal static List<GeocodingModel> Cities { get; set; } = new List<GeocodingModel>();
        internal static int Index { get; set; }
        internal static WeatherModel Forecast { get; set; } = new WeatherModel();

        /// <summary>
        /// This is the method applications should call to run the library and make weather forecast queries. Runs asynchronously,
        /// so graphical user interfaces will remain responsive.
        /// </summary>
        /// <param name="userInterface">Reference to an instance of a class which implements the IUserInterface interface.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task RunAsync(IUserInterface userInterface)
        {
            UI = userInterface;

            // Initialize the API key and HTTP client.
            InitializeLibrary();

            // Clear UserInput every time the library is run.
            UserInput = "";
            do
            {
                // Ask the user for the city whose weather forecast they wish to know.
                await GeocodingLogic.GetCoordinatesAsync();

                // Get the weather forecast and print it to the user.
                await OpenWeatherMapApi.GetWeatherForecastAsync();
                PrintForecast();

                // Ask the user whether they want to look up another forecast or exit the program.
                UI.PrintToUser("Syötä jonkin toisen kaupungin nimi tehdäksesi uusi haku,");
                UI.PrintToUser("tai jätä kenttä tyhjäksi ja paina ENTER poistuaksesi ohjelmasta.");
                UserInput = UI.GetUserInput();
            } while (!string.IsNullOrWhiteSpace(UserInput));
        }

        /// <summary>
        /// Initializes the library by calling all the necessary initialization methods.
        /// </summary>
        private static void InitializeLibrary()
        {
            ReadApiKey();
            ApiHelper.InitializeClient();
        }

        /// <summary>
        /// Reads the API key from appsettings.json.
        /// </summary>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        private static void ReadApiKey()
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                try
                {
                    var Configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: false)
                        .Build();

                    ApiKey = Configuration["ApiKeyOpenWeatherMap"];
                }
                catch (FileNotFoundException e)
                {
                    throw new FileNotFoundException("appsettings.json-tiedostoa ei löytynyt!", e);
                }

                if (string.IsNullOrWhiteSpace(ApiKey))
                {
                    throw new KeyNotFoundException("API-avainta ei löytynyt appsettings.json-tiedostosta!");
                }
            }
        }

        /// <summary>
        /// Displays the acquired weather forecast data to the user.
        /// </summary>
        private static void PrintForecast()
        {
            UI.PrintToUser($"\nPaikkakunnan {DetermineName()} viikon sääennuste on:");
            foreach (var day in Forecast.Daily)
            {
                var dateTime = DateTimeOffset.FromUnixTimeSeconds(day.UnixTimeUtc + Forecast.TimeZoneOffset);
                int maxTemperature = (int)Math.Round(day.Temperature.Max, 0, MidpointRounding.AwayFromZero);
                int minTemperature = (int)Math.Round(day.Temperature.Min, 0, MidpointRounding.AwayFromZero);
                UI.PrintToUser($"{dateTime.Day}.{dateTime.Month}.{dateTime.Year}: max {maxTemperature} °C, min {minTemperature} °C");
            }
            UI.PrintToUser("");
        }

        /// <summary>
        /// Determines which city name that's available should be printed to the user. Prioritizes Finnish, followed by English.
        /// </summary>
        /// <returns>The name of the city.</returns>
        private static string DetermineName()
        {
            if (!string.IsNullOrWhiteSpace(Cities[Index].LocalNames.Finnish))
            {
                return Cities[Index].LocalNames.Finnish;
            }
            else if (!string.IsNullOrWhiteSpace(Cities[Index].LocalNames.English))
            {
                return Cities[Index].LocalNames.English;
            }
            else
            {
                return Cities[Index].Name;
            }
        }
    }
}
