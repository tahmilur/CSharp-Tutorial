using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"c:\test\test.dat";
            string spath = @"c:\test\test1.dat";

            using (FileStream stream = File.Create(path))
            {
                string data = "This is a test data";
                byte[] fdata = Encoding.Default.GetBytes(data);

                stream.Write(fdata, 0, fdata.Length);
            }

            // using stream writer
            using (StreamWriter stream = File.CreateText(spath))
            {
                string data = "This is a test data";
                stream.Write(data);
            }

            // Read byte
            using (FileStream stream = File.OpenRead(path))
            {
                byte[] data = new byte[stream.Length];

                for (int i = 0; i < stream.Length; i++)
                {
                    data[i] = (byte)stream.ReadByte();
                }

                Console.WriteLine(Encoding.Default.GetString(data));
            }

            using (StreamReader streamWriter = File.OpenText(path))
            {
                Console.WriteLine(streamWriter.ReadLine()); // Displays: MyValue
            }

            // Compressing data with a GZipStream
            string folder = @"c:\temp";
            string uncompressedFilePath = Path.Combine(folder, "uncompressed.dat");
            string compressedFilePath = Path.Combine(folder, "compressed.gz");
            byte[] dataToCompress = Enumerable.Repeat((byte)'a', 1024 * 1024).ToArray();

            using (FileStream uncompressedFileStream = File.Create(uncompressedFilePath))
            {
                uncompressedFileStream.Write(dataToCompress, 0, dataToCompress.Length);
            }

            using (FileStream compressedFileStream = File.Create(compressedFilePath))
            {
                using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                {
                    compressionStream.Write(dataToCompress, 0, dataToCompress.Length);
                }
            }

            FileInfo uncompressedFile = new FileInfo(uncompressedFilePath);
            FileInfo compressedFile = new FileInfo(compressedFilePath);

            Console.WriteLine(uncompressedFile.Length); // Displays 1048576
            Console.WriteLine(compressedFile.Length); // Displays 1052


            // Using a BufferedStream
            path = @"c:\temp\bufferedStream.txt";

            using (FileStream fileStream = File.Create(path))
            {
                using (BufferedStream bufferedStream = new BufferedStream(fileStream))
                {
                    using (StreamWriter streamWriter = new StreamWriter(bufferedStream))
                    {
                        streamWriter.WriteLine("A line of text.");
                    }
                }
            }

            Console.Read();
        }
    }
}
