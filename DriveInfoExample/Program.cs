using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveInfoExample
{
    class Program
    {
        static void Main(string[] args)
        {
            DriveInfo[] drivesInfo = DriveInfo.GetDrives();

            foreach (var driveInfo in drivesInfo)
            {
                Console.WriteLine("Drive {0}", driveInfo.Name);
                Console.WriteLine("  File type: {0}", driveInfo.DriveType);

                if (driveInfo.IsReady)
                {
                    Console.WriteLine("  Volume label: {0}", driveInfo.VolumeLabel);
                    Console.WriteLine("  File system: {0}", driveInfo.DriveFormat);
                    Console.WriteLine(
                        "  Available space to current user:{0, 15} bytes",
                        driveInfo.AvailableFreeSpace);

                    Console.WriteLine(
                        "  Total available space:          {0, 15} bytes",
                        driveInfo.TotalFreeSpace);

                    Console.WriteLine(
                        "  Total size of drive:            {0, 15} bytes ",
                        driveInfo.TotalSize);
                }

                Console.WriteLine();
            }

            Console.Read();
        }

        private static string GetSize(long byteData)
        {
            string result = string.Empty;

            result = (byteData / 1024).ToString() + " KB";
            result = (byteData / 1024 / 1024).ToString() + " MB";
            //result = (byteData / 1024 / 1024 / 1024).ToString() + " GB";

            return result;
        }
    }
}