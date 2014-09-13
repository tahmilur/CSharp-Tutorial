using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelegateExample
{
    /// <summary>
    /// This class is to perform gien two number -
    /// Divide, Multiply, Add, Subtract, Remainder
    /// </summary>
    public class SimpleMath
    {
        private delegate void Del(string message);
        Del handler;

        public SimpleMath()
        {
            handler = DelegateMethod;
        }

        public int Add(int x1, int x2)
        {
            int result = x1 + x2;
            handler("Sum: " + result);
            return result;
        }

        public int Subtract(int x1, int x2)
        {
            int result = x1 - x2;
            handler("Subtract: " + result);
            return result;
        }

        public int Multiply(int x1, int x2)
        {
            int result = x1 * x2;
            handler("Multiply: " + result);
            return result;
        }

        public int Divide(int x1, int x2)
        {
            var result = x2 != 0 ? (x1 / x2) : 0;
            handler("Divide: " + result);
            return result;
        }

        public int Remainder(int x1, int x2)
        {
            int result = x1 % x2;
            handler("Remainder: " + result);
            return result;
        }

        private void DelegateMethod(string message)
        {
            Console.WriteLine(message);
        }
    }
}
