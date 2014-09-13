using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelegateExample
{
    /// <summary>
    /// Main program
    /// </summary>
    class Program
    {
        // declare delegate
        public delegate int DelMath(int x1, int x2);

        /// <summary>
        /// Delegates examples
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // input values
            int x1 = 50;
            int x2 = 20;

            // Instantiate class
            var sm = new SimpleMath();

            // Create an instance of the delegate.
            // 1st way 
            DelMath add = new DelMath(sm.Add);
            // 2nd way
            DelMath subtract = sm.Subtract;
            DelMath multiply = sm.Multiply;
            DelMath divide = sm.Divide;
            DelMath remainder = sm.Remainder;

            // call Add
            int result = add(x1, x2);

            Console.WriteLine("------------------------------------------");

            // Add all delegates
            DelMath allMethodsDelegate = add + subtract + multiply + divide + remainder;

            // no of invocation of a delegate
            int invocationCount = allMethodsDelegate.GetInvocationList().GetLength(0);
            Console.WriteLine("Invocation Count: " + invocationCount.ToString());

            // Call 
            allMethodsDelegate(x1, x2);

            Console.WriteLine("------------------------------------------");

            //remove Method Add
            allMethodsDelegate -= add;

            // Call Again
            allMethodsDelegate(x1, x2);

            // Write a line in the console in debug mode
            Console.WriteLine();
            Console.WriteLine("Press any key to exit.");            
            Console.ReadKey(); 
        }
    }
}
