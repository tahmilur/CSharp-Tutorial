using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegularExpressionExample
{
    class Program
    {
        static void Main(string[] args)
        {
            RegexOptions options = RegexOptions.None;
            Regex re = new Regex(@"[ ]{2,}", options);

            string input = "1 2 3  5  6 7   8   10";
            string result = re.Replace(input, " ");

            Console.WriteLine(result);

            Console.Read();
        }
    }
}
