using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExample
{
    class Program
    {
        static void Main(string[] args)
        {          
            // Path example
            
            foreach (var item in Path.GetInvalidFileNameChars())
            {
                Console.Write(item + " ");
            }

            Console.WriteLine();

            foreach (var item in Path.GetInvalidPathChars())
            {
                Console.Write(item + " ");
            }

            Console.WriteLine();

            Console.WriteLine("RandomFileName: " + Path.GetRandomFileName());

            Console.WriteLine("DirectorySeparatorChar: " + Path.DirectorySeparatorChar);

            Console.Read();
        }

        private static void CopyFile()
        {
            string path = @"c:\temp\test.txt";
            string destPath = @"c:\temp\destTest.txt";

            File.CreateText(path).Close();
            File.Copy(path, destPath);

            FileInfo fileInfo = new FileInfo(path);
            fileInfo.CopyTo(destPath);
        }

        private static void MoveFile()
        {
            // Moving a file
            string path = @"c:\temp\test.txt";
            string destPath = @"c:\temp\destTest.txt";

            File.CreateText(path).Close();
            File.Move(path, destPath);

            FileInfo fileInfo = new FileInfo(path);
            fileInfo.MoveTo(destPath);
        }
    }
}
