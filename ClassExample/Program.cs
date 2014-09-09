using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Author: Tahmilur
// Creation Date : 9-9-2014

namespace ClassExample
{
    
    /// <summary>
    /// Class definition. 
    /// </summary>
    public class MyMath
    {
        // Fields
        private int number;

        // Constants
        public const int MAXVALUE = 5000;

        // Properties
        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        // Methods
        public int Multiply(int num1, int num2)
        {
            return num1 * num2;
        }

        public double Multiply(double num1, double num2)
        {
            return num1 * num2;
        }

        public decimal Multiply(decimal num1, decimal num2)
        {
            return num1 * num2;
        }
        
        // Instance constructor
        public MyMath()
        {
            number = 0;
        }
    }

    // Another class definition. This one contains 
    // the Main method, the entry point for the program. 
    class Program
    {
        static void Main(string[] args)
        {
            // Create an object of type MyCustomClass.
            MyMath myClass = new MyMath();

            // Set the value of a public property.
            myClass.Number = 2500;

            // Call a public method. 
            int result = myClass.Multiply(4, 4);
        }
    }
}
