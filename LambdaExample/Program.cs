using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<int, int> myDelegate = delegate(int i) { return i * 2; };
            Func<int, int> myDelegate1 = x => x * 2;

            Func<int, int, int> mySum = (int x, int y) => { return (x + y); };
            Func<int> mytest = () => 20;

            Console.WriteLine(myDelegate(21)); // Displays 42
            Console.WriteLine(myDelegate1(10)); // Displays 4
            Console.WriteLine(mySum(10, 100)); // Displays 4

            Console.Read();
        }
    }
}
