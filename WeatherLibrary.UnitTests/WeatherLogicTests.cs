using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeatherLibrary.Logic;

namespace WeatherLibrary.UnitTests
{
    /// <summary>
    /// Contains unit tests for the WeatherLogic class in WeatherLibrary.
    /// </summary>
    [TestClass]
    public class WeatherLogicTests
    {
        /// <summary>
        /// Tests that the method RunAsync throws an exception if it's called while passing in a null reference.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        [TestMethod]
        public async Task RunAsync_ArgumentIsNull_ThrowsArgumentNullException()
        {
            IUserInterface nullInterface = null;

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => WeatherLogic.RunAsync(nullInterface));
        }
    }
}
