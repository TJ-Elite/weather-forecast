using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WeatherLibrary.Api
{
    /// <summary>
    /// Contains everything necessary to set up an HTTP client necessary to make API calls to openweathermap.org.
    /// </summary>
    internal static class ApiHelper
    {
        private const string OpenWeatherMapBaseAddress = "https://api.openweathermap.org/";

        // Note: HttpClientFactory is the new recommended way for making HTTP calls and would be one of the improvements that could be
        // made to this application.
        internal static readonly HttpClient apiClient = new HttpClient();

        private static bool _initialized;

        internal static bool Initialized
        {
            get => _initialized;
            private set => _initialized = value;
        }

        /// <summary>
        /// Initializes the HTTP client. Should be run once before making any API calls.
        /// </summary>
        internal static void InitializeClient()
        {
            if (!Initialized)
            {
                apiClient.BaseAddress = new Uri(OpenWeatherMapBaseAddress);
                apiClient.DefaultRequestHeaders.Accept.Clear();
                apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Initialized = true;
            }
        }
    }
}
