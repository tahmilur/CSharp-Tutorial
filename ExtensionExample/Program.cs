using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionExample
{
    class Program
    {
        static void Main(string[] args)
        {
            int x = 20;
            Console.WriteLine(x.Multiply(20));

            Console.Read();
        }
    }

    public static class IntExtension
    {
        public static int Multiply(this int x, int y)
        {
            return x * y;
        }
    }
}
