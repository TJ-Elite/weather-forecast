using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeatherLibrary.Logic;
using WeatherLibrary.Models;

namespace WeatherLibrary.UnitTests
{
    /// <summary>
    /// Contains unit tests for the GeocodingLogic class in WeatherLibrary.
    /// </summary>
    [TestClass]
    public class GeocodingLogicTests
    {
        /// <summary>
        /// Tests that the method ConstructPhrase returns a correctly formed string when provided with the data for Oulu, Finland.
        /// </summary>
        [TestMethod]
        public void ConstructPhrase_IsGivenDataForOuluFi_ConstructsTheCorrectString()
        {
            var localNames = new LocalNamesModel()
            {
                English = "Oulu",
                Finnish = "Oulu"
            };
            var city = new GeocodingModel()
            {
                Name = "Oulu",
                LocalNames = localNames,
                Latitude = 65.0124,
                Longitude = 25.4682,
                Country = "FI"
            };
            int index = 0;
            string expected = "1. Oulu, FI, 65°1'N, 25°28'E";

            string result = GeocodingLogic.ConstructPhrase(city, index);

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Tests that the method ConstructPhrase returns a correctly formed string when provided with the data for York, Nebraska, USA.
        /// </summary>
        [TestMethod]
        public void ConstructPhrase_IsGivenDataForYorkUsNe_ConstructsTheCorrectString()
        {
            var localNames = new LocalNamesModel()
            {
                English = "York"
            };
            var city = new GeocodingModel()
            {
                Name = "York",
                LocalNames = localNames,
                Latitude = 40.8681,
                Longitude = -97.592,
                Country = "US",
                State = "NE"
            };
            int index = 3;
            string expected = "4. York, US (NE), 40°52'N, 97°36'W";

            string result = GeocodingLogic.ConstructPhrase(city, index);

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Tests that the extension method GetArcminutePart returns the value 30 when the input value is 61.5.
        /// </summary>
        [TestMethod]
        public void GetArcminutePart_Input61_5_Returns30()
        {
            double degrees = 61.5;
            uint expected = 30;

            uint result = degrees.GetArcminutePart();

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Tests that the extension method GetArcminutePart returns the value 49 when the input value is -37.81.
        /// </summary>
        [TestMethod]
        public void GetArcminutePart_InputNegative37_81_Returns49()
        {
            double degrees = -37.81;
            uint expected = 49;

            uint result = degrees.GetArcminutePart();

            Assert.AreEqual(expected, result);
        }
    }
}
