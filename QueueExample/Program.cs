using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Queue<int> stack = new Queue<int>();
            stack.Enqueue(3);
            stack.Enqueue(7);
            stack.Enqueue(4);

            int x = stack.Dequeue();
            stack.Enqueue(9);

            int y = stack.Dequeue();
            int z = stack.Dequeue();

            Console.WriteLine("Values are x: {0}  y: {1} z: {2}", x, y, z);

            //
            Console.WriteLine("Please enter any key to exit.");
            Console.ReadLine();
        }
    }
}
