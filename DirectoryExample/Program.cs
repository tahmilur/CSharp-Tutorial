using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryExample
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\Program Files");
            ListDirectories(directoryInfo, "*a*", 3, 0);

            foreach (string file in Directory.GetFiles(@"C:\Windows"))
            {
                Console.WriteLine(file);
            }

            Console.WriteLine("------------------------------------");

            directoryInfo = new DirectoryInfo(@"C:\Windows");

            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                Console.WriteLine(fileInfo.FullName);
            }

            Console.Read();
        }

        private static void DirectoryEnumeration()
        {
            string dirPath = @"c:\windows";

            try
            {
                List<string> dirs = new List<string>(Directory.EnumerateDirectories(dirPath));

                foreach (var dir in dirs)
                {
                    Console.WriteLine("{0}", dir.Substring(dir.LastIndexOf("\\") + 1));
                }
                Console.WriteLine("{0} directories found.", dirs.Count);
            }
            catch (UnauthorizedAccessException UAEx)
            {
                Console.WriteLine(UAEx.Message);
            }
            catch (PathTooLongException PathEx)
            {
                Console.WriteLine(PathEx.Message);
            }
        }

        private static void SetDirectoryAccess()
        {
            // Setting access control for a directory
            DirectoryInfo di = new DirectoryInfo(@"d:/directorytest/example2");

            if (di.Exists)
            {
                DirectorySecurity ds = di.GetAccessControl();
                ds.AddAccessRule(new FileSystemAccessRule("everyone", FileSystemRights.ReadAndExecute, AccessControlType.Allow));

                di.SetAccessControl(ds);
            }
        }

        private static void CreateDirectory()
        {
            // Create directory
            var directory = Directory.CreateDirectory(@"d:/directorytest/example1");

            DirectoryInfo di = new DirectoryInfo(@"d:/directorytest/example2");
            di.Create();
        }

        private static void ListDirectories(DirectoryInfo directoryInfo, string searchPattern, int maxLevel, int currentLevel)
        {
            if (currentLevel >= maxLevel)
            {
                return;
            }

            string indent = new string('-', currentLevel);

            try
            {
                DirectoryInfo[] subDirectories = directoryInfo.GetDirectories(searchPattern);

                foreach (DirectoryInfo subDirectory in subDirectories)
                {
                    Console.WriteLine(indent + subDirectory.Name);
                    ListDirectories(subDirectory, searchPattern, maxLevel, currentLevel + 1);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // You don't have access to this folder. 
                Console.WriteLine(indent + "Can't access: " + directoryInfo.Name);
                return;
            }
            catch (DirectoryNotFoundException)
            {
                // The folder is removed while iterating
                Console.WriteLine(indent + "Can't find: " + directoryInfo.Name);
                return;
            }
        }
    }
}
