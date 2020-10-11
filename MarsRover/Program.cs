using MarsRover.Command;
using MarsRover.NasaRover;
using MarsRover.Surface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarsRover
{
    public class Program
    {
        private static List<string> commandList;
        static void Main(string[] args)
        {
            #region DI Implementation

            var serviceProvider = new ServiceCollection()
                .AddSingleton<ISurface, Plateau>()
                .AddSingleton<IRoverManager, RoverManager>()
                .BuildServiceProvider();

            #endregion

            DisplayTestData();

            Console.WriteLine("Would you like to enter data? Otherwise test data will be considered. (Y/N)");

            var answer = Console.ReadKey();
            while (answer.Key != ConsoleKey.Y && answer.Key != ConsoleKey.N)
            {
                Console.WriteLine("Please only type Y or N.");
                answer = Console.ReadKey();
            }

            GetOrSetCommands(answer.Key == ConsoleKey.N);

            try
            {
                // Start to explore Plateau
                var command = new Command.Command(serviceProvider);
                commandList.ForEach(c => command.SendCommand(c));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Oops Error. {ex.Message}");
            }

            Console.ReadLine();
        }

        /// <summary>
        /// According to user choice, It sets commands to explore Plateau
        /// </summary>
        /// <param name="isTestData"></param>
        private static void GetOrSetCommands(bool isTestData)
        {
            Console.WriteLine();
            if (isTestData)
            {
                commandList = new List<string>
                {
                    "5 5", // Plateau
                    "1 2 N", "LMLMLMLMM", // Rover1
                    "3 3 E", "MMRMMRMRRM" // Rover2
                };
                return;
            }

            // Get data from user
            Console.WriteLine("Please enter Plateau size");
            var plateauSize = Console.ReadLine().Trim().ToString();
            Console.WriteLine("Please enter Rover1 initial position and direction");
            var rover1InıtialPositionAndDirection = Console.ReadLine().Trim().ToString();
            Console.WriteLine("Please enter Rover1 movements to explore Plateau");
            var rover1Movements = Console.ReadLine().Trim().ToString();
            Console.WriteLine("Please enter Rover2 initial position and direction");
            var rover2InıtialPositionAndDirection = Console.ReadLine().Trim().ToString();
            Console.WriteLine("Please enter Rover2 movements to explore Plateau");
            var rover2Movements = Console.ReadLine().Trim().ToString();

            commandList = new List<string>
            {
                plateauSize,
                rover1InıtialPositionAndDirection,rover1Movements,
                rover2InıtialPositionAndDirection,rover2Movements
            };
        }

        private static void DisplayTestData()
        {
            Console.WriteLine("****** Test Input ******");
            Console.WriteLine("Plateau Size:                  5 5");
            Console.WriteLine("Rover1 Position and Direction: 1 2 N");
            Console.WriteLine("Rover1 Movements to Explore:   LMLMLMLMM");
            Console.WriteLine("Rover2 Position and Direction: 3 3 N");
            Console.WriteLine("Rover2 Movements to Explore:   MMRMMRMRRM");
            Console.WriteLine("************************");

        }
    }
}
