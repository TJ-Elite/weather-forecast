using System;
using System.Threading.Tasks;
using WeatherLibrary;
using WeatherLibrary.Logic;

namespace WeatherConsole
{
    internal static class Program
    {
        private static async Task Main()
        {
            // We have to create an instance of a class that implements the IUserInterface inteface
            // and pass a reference to it to WeatherLibrary.
            IUserInterface userInterface = new UserInterface();
            try
            {
                await WeatherLogic.RunAsync(userInterface);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nOhjelmassa tapahtui odottamaton virhe ja se lakkasi toimimasta!");
                Console.WriteLine("Virheviesti:");
                Console.WriteLine(e.Message);
                Console.WriteLine("\nPaina mitä tahansa näppäintä poistuaksesi ohjelmasta.");
                Console.ReadKey(true);
            }
        }
    }
}
