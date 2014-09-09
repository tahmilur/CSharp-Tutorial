using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloWorld
{
    /// <summary>
    /// Main program
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main entry point of a C# program. This program displays the string 
        /// "Hello World!" on the screen.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Write a line in the console in debug mode
            Console.WriteLine("Hello World!");
            Console.WriteLine("Press any key to exit.");

            // Wait for user input in console
            Console.ReadKey();
        }
    }
}