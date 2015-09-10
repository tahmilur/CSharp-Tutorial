using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;
using Microsoft.CSharp;

/*
#warning This code is obsolete

#if DEBUG
#error Debug build is not allowed
#endif
*/

namespace ThreadExample
{
    class Program
    {
        [ThreadStatic]
        public static int _Field;

        public static ThreadLocal<int> _localField =
                                                    new ThreadLocal<int>(() =>
                                                    {
                                                        return Thread.CurrentThread.ManagedThreadId;
                                                    });

        static void Main(string[] args)
        {
            try
            {
                System.Console.WriteLine(" Lists of Examples: ");
                System.Console.WriteLine("");
                System.Console.WriteLine(" 1. ThreadExample");
                System.Console.WriteLine(" 2. ThreadStopExample");
                System.Console.WriteLine(" 3. TaskExample");
                System.Console.WriteLine(" 4. ParallelExample");
                System.Console.WriteLine(" 5. ParallelQueryExample");
                System.Console.WriteLine(" 6. ForAllAsParallelExample");
                System.Console.WriteLine(" 7. BlockingCollectionExample");
                System.Console.WriteLine(" 8 ConcurrentBagExample");
                System.Console.WriteLine(" 9. EnumeratingConcurrentBagExample");
                System.Console.WriteLine("10. ConcurrentStackExample");
                System.Console.WriteLine("11. ConcurrentQueueExample");
                System.Console.WriteLine("12. ConcurrentDictionaryExample");
                System.Console.WriteLine("13. NullCoalasingExample");
                System.Console.WriteLine("14. ConditionalOperatorExample");
                System.Console.WriteLine("15. SwitchCaseExample");
                System.Console.WriteLine("16. ForLoopExample");

                System.Console.WriteLine("");
                System.Console.Write("Please enter INTEGER example no: ");

                int userOption = int.Parse(System.Console.ReadLine());

                switch (userOption)
                {
                    case 1: ThreadExample(); break;
                    case 2: ThreadStopExample(); break;
                    case 3: TaskExample(); break;
                    case 4: ParallelExample(); break;
                    case 5: ParallelQueryExample(); break;
                    case 6: ForAllAsParallelExample(); break;
                    case 7: BlockingCollectionExample(); break;
                    case 8: ConcurrentBagExample(); break;
                    case 9: EnumeratingConcurrentBagExample(); break;
                    case 10: ConcurrentStackExample(); break;
                    case 11: ConcurrentQueueExample(); break;
                    case 12: ConcurrentDictionaryExample(); break;
                    case 13: NullCoalasingExample(); break;
                    case 14: ConditionalOperatorExample(); break;
                    case 15: SwitchCaseExample(); break;
                    case 16: ForLoopExample(); break;

                    default:
                        break;
                }
            }
            catch (FormatException e)
            {
                Console.WriteLine("Message: {0}", e.Message);
                Console.WriteLine("StackTrace: {0}", e.StackTrace);
                Console.WriteLine("HelpLink: {0}", e.HelpLink);
                Console.WriteLine("InnerException: {0}", e.InnerException);
                Console.WriteLine("TargetSite: {0}", e.TargetSite);
                Console.WriteLine("Source: {0}", e.Source);
            }

            System.Console.WriteLine("");
            System.Console.WriteLine("Press any key to exit. ");
            System.Console.ReadLine();
        }

        /// <summary>
        /// 1. Thread Example 
        /// </summary>
        private static void ThreadExample()
        {
            // Example 1: Without parameter
            Thread t1 = new Thread(new ThreadStart(ThreadMethod));
            t1.Start();

            System.Console.WriteLine("T1 IsBackground: {0}", t1.IsBackground);

            for (int i = 0; i < 10; i++)
            {
                System.Console.WriteLine("Main thread Proc: {0}", i);
                Thread.Sleep(100);
            }

            t1.Join();

            // Example 2: With parameter
            Thread t2 = new Thread(new ParameterizedThreadStart(ThreadMethodWithParameter));
            t2.Start(10);
            t2.Join();

            // Example 3: Thread Attribute
            new Thread(new ThreadStart(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    _Field++;
                    Console.WriteLine("Thread Attribute - A: {0}", _Field);
                }

            })).Start();

            new Thread(new ThreadStart(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    _Field++;
                    Console.WriteLine("Thread Attribute - B: {0}", _Field);
                }

            })).Start();

            // Example 4: Local Thread field value
            new Thread(new ThreadStart(() =>
            {
                Console.WriteLine("Local Thread Attribute - A: {0}", _localField.Value);

                for (int i = 0; i < _localField.Value; i++)
                {
                    Console.WriteLine("Local Thread A: {0}", i);
                }

            })).Start();

            new Thread(new ThreadStart(() =>
            {
                Console.WriteLine("Local Thread Attribute - B: {0}", _localField.Value);

                for (int i = 0; i < _localField.Value; i++)
                {
                    Console.WriteLine("Local Thread B: {0}", i);
                }

            })).Start();
            //

            // Example 5:Thread Pool
            // ThreadPool.QueueUserWorkItem(new WaitCallback(WaitCallbackProc));

            ThreadPool.QueueUserWorkItem((s) =>
            {
                Console.WriteLine("Working on a thread from threadpool");
            });
        }

        private static void ThreadStopExample()
        {
            bool stopped = false;

            Thread t2 = new Thread(new ThreadStart(() =>
            {
                while (!stopped)
                {
                    Console.WriteLine("Running...");
                    Thread.Sleep(1000);
                }
            }));

            t2.Start();

            System.Console.WriteLine("Press any key to exit");
            System.Console.ReadLine();

            stopped = true;
            t2.Join();
        }

        private static void TaskExample()
        {
            // Task example
            Task<int> tsk = Task.Run(() =>
            {
                for (int x = 0; x < 10; x++)
                {
                    Console.Write('*');
                }
                return 42;

            }).ContinueWith((i) =>
            {
                return i.Result * 2;
            });

            // tsk.Wait();
            System.Console.WriteLine("Task Result: " + tsk.Result.ToString());

            Task<int> t2 = Task.Run(() =>
            {
                return 42;
            });

            t2.ContinueWith((i) =>
            {
                Console.WriteLine("Canceled");
            }, TaskContinuationOptions.OnlyOnCanceled);

            t2.ContinueWith((i) =>
            {
                Console.WriteLine("Faulted");
            }, TaskContinuationOptions.OnlyOnFaulted);

            var completedTask = t2.ContinueWith((i) =>
            {
                Console.WriteLine("Completed");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            completedTask.Wait();

            // Attaching child tasks to a parent task
            Task<Int32[]> parent = Task.Run(() =>
            {
                var results = new Int32[3];
                new Task(() => results[0] = 0,
                    TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[1] = 1,
                    TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[2] = 2,
                    TaskCreationOptions.AttachedToParent).Start();

                return results;
            });

            var finalTask = parent.ContinueWith(
              parentTask =>
              {
                  foreach (int i in parentTask.Result)
                      Console.WriteLine(i);
              });

            finalTask.Wait();

            // Using a TaskFactory
            Task<Int32[]> parent1 = Task.Run(() =>
            {
                var results = new Int32[3];

                TaskFactory tf = new TaskFactory(TaskCreationOptions.AttachedToParent, TaskContinuationOptions.ExecuteSynchronously);

                tf.StartNew(() => results[0] = 0);
                tf.StartNew(() => results[1] = 1);
                tf.StartNew(() => results[2] = 2);
                return results;
            });

            var finalTask1 = parent1.ContinueWith(
               parentTask =>
               {
                   foreach (int i in parentTask.Result)
                   {
                       Console.WriteLine(i);
                   }

               });

            finalTask1.Wait();

            // Task wait all
            Task[] tasks = new Task[3];

            tasks[0] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("1");
                return 1;
            });
            tasks[1] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("2");
                return 2;
            });
            tasks[2] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("3");
                return 3;
            });

            Task.WaitAll(tasks);

            //
            Task<int>[] tasks1 = new Task<int>[3];

            tasks1[0] = Task.Run(() => { Thread.Sleep(2000); return 1; });
            tasks1[1] = Task.Run(() => { Thread.Sleep(1000); return 2; });
            tasks1[2] = Task.Run(() => { Thread.Sleep(3000); return 3; });

            while (tasks1.Length > 0)
            {
                int i = Task.WaitAny(tasks1);
                Task<int> completedTask1 = tasks1[i];
                Console.WriteLine("completedTask1: " + completedTask1.Result);

                var temp = tasks1.ToList();
                temp.RemoveAt(i);
                tasks1 = temp.ToArray();
            }
        }

        private static void ParallelExample()
        {
            // Using Parallel.For  and Parallel.Foreach 
            Parallel.For(0, 10, i =>
            {
                Console.WriteLine("Parallel For: " + i.ToString());
                Thread.Sleep(1000);
            });

            var numbers = Enumerable.Range(0, 10);

            Parallel.ForEach(numbers, i =>
            {
                Console.WriteLine("Parallel ForEach: " + i.ToString());
                Thread.Sleep(1000);
            });

            // Parallel.Break
            ParallelLoopResult result1 = Parallel.
            For(0, 1000, (int i, ParallelLoopState loopState) =>
            {
                if (i == 500)
                {
                    Console.WriteLine("Breaking loop");
                    loopState.Break();
                    //loopState.Stop();
                }
                return;
            });

            System.Console.WriteLine("ParallelLoopResult: {0} -- {1}", result1.IsCompleted, result1.LowestBreakIteration);

            // example of an  asynchronous method.
            string result = DownloadContent().Result;

            Console.WriteLine(result);
        }

        private static void ParallelQueryExample()
        {
            // Unordered parallel query
            var numbers1 = Enumerable.Range(0, 10);
            var parallelResult = numbers1.AsParallel().AsOrdered()
                .Where(i => i % 2 == 0)
                .ToArray();

            foreach (int i in parallelResult)
                Console.WriteLine(i);

            //  Making a parallel query sequential
            var numbers2 = Enumerable.Range(0, 20);

            var parallelResult2 = numbers2.AsParallel().AsOrdered()
                .Where(i => i % 2 == 0).AsSequential();

            foreach (int i in parallelResult2.Take(5))
                Console.WriteLine(i);

            // Catching AggregateException
            var numbers3 = Enumerable.Range(0, 20);

            try
            {
                var parallelResult3 = numbers3.AsParallel()
                    .Where(i => IsEven(i));

                parallelResult3.ForAll(e => Console.WriteLine(e));
            }
            catch (AggregateException e)
            {
                Console.WriteLine("There where {0} exceptions",
                                    e.InnerExceptions.Count);
            }
        }

        private static void ForAllAsParallelExample()
        {
            // ForAll examples
            var numbers4 = Enumerable.Range(0, 20);

            var parallelResult4 = numbers4.AsParallel().AsOrdered()
                .Where(i => i % 2 == 0);

            parallelResult4.ForAll(e => Console.WriteLine(e));
        }

        private static void BlockingCollectionExample()
        {
            // BlockingCollection
            BlockingCollection<string> col = new BlockingCollection<string>();

            Task read = Task.Run(() =>
            {
                /*
                while (true)
                {
                    Console.WriteLine(col.Take());
                }*/

                foreach (string v in col.GetConsumingEnumerable())
                    Console.WriteLine(v);
            });

            Task write = Task.Run(() =>
            {
                while (true)
                {
                    string s = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(s)) break;
                    col.Add(s);
                }
            });

            write.Wait();
        }

        private static void ConcurrentBagExample()
        {
            ConcurrentBag<int> bag = new ConcurrentBag<int>();

            bag.Add(42);
            bag.Add(21);

            int result;
            if (bag.TryTake(out result))
                Console.WriteLine(result);

            if (bag.TryPeek(out result))
                Console.WriteLine("There is a next item: {0}", result);
        }

        private static void EnumeratingConcurrentBagExample()
        {
            ConcurrentBag<int> bag = new ConcurrentBag<int>();

            Task.Run(() =>
            {
                bag.Add(42);
                Thread.Sleep(1000);
                bag.Add(21);
            });

            Task.Run(() =>
            {
                foreach (int i in bag)
                    Console.WriteLine(i);
            }).Wait();
        }

        private static void ConcurrentStackExample()
        {
            ConcurrentStack<int> stack = new ConcurrentStack<int>();

            stack.Push(42);

            int result;
            if (stack.TryPop(out result))
                Console.WriteLine("Popped: {0}", result);

            stack.PushRange(new int[] { 1, 2, 3 });

            int[] values = new int[2];
            stack.TryPopRange(values);

            foreach (int i in values)
                Console.WriteLine(i);
        }

        private static void ConcurrentQueueExample()
        {
            ConcurrentQueue<int> queue = new ConcurrentQueue<int>();
            queue.Enqueue(42);

            int result;
            if (queue.TryDequeue(out result))
                Console.WriteLine("Dequeued: {0}", result);
        }

        private static void ConcurrentDictionaryExample()
        {
            var dict = new ConcurrentDictionary<string, int>();
            if (dict.TryAdd("k1", 42))
            {
                Console.WriteLine("Added");
            }

            if (dict.TryUpdate("k1", 21, 42))
            {
                Console.WriteLine("42 updated to 21");
            }

            dict["k1"] = 42; // Overwrite unconditionally

            int r1 = dict.AddOrUpdate("k1", 3, (s, i) => i * 2);
            int r2 = dict.GetOrAdd("k2", 3);
        }

        private static void NullCoalasingExample()
        {
            /*
            int? x = null;
            int y = x ?? -1;
            */

            int? x = null;
            int? z = null;
            int y = x ?? z ?? -1;

            Console.WriteLine(y);
        }

        private static void ConditionalOperatorExample()
        {
            int? x = null;
            int y = x != null ? 1 : 0;

            Console.WriteLine(y);
        }

        private static void SwitchCaseExample()
        {
            char input = 'c';

            switch (input)
            {
                case 'a':
                case 'e':
                case 'i':
                case 'o':
                case 'u':
                    {
                        Console.WriteLine("Input is a vowel");
                        break;
                    }
                case 'y':
                    {
                        Console.WriteLine("Input is sometimes a vowel.");
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Input is a consonant");
                        break;
                    }
            }
        }

        private static void DebugExample()
        {
            Stream outputFile = File.Create("tracefile.txt");
            TextWriterTraceListener textListener = new TextWriterTraceListener(outputFile);

            Debug.WriteLine("Starting application");
            Debug.Indent();
            int i = 1 + 2;
            Debug.Assert(i == 3);
            Debug.WriteLineIf(i > 0, "i is greater than 0");

            TraceSource traceSource = new TraceSource("myTraceSource", SourceLevels.All);
            traceSource.Listeners.Clear();
            traceSource.Listeners.Add(textListener);

            traceSource.TraceInformation("Tracing application..");
            traceSource.TraceEvent(TraceEventType.Critical, 0, "Critical trace");
            traceSource.TraceData(TraceEventType.Information, 1, new object[] { "a", "b", "c" });

            traceSource.Flush();
            traceSource.Close();
        }

        private static void PerformanceMonitorExample()
        {
            Console.WriteLine("Press escape key to stop");
            using (PerformanceCounter pc = new PerformanceCounter("Memory", "Available Bytes"))
            {
                string text = "Available memory: ";

                Console.Write(text);
                do
                {
                    while (!Console.KeyAvailable)
                    {
                        Console.Write(pc.RawValue);
                        Console.SetCursorPosition(text.Length, Console.CursorTop);
                    }
                } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
            }
        }

        private static void ListingDriveInformationExample()
        {
            // Listing drive information
            DriveInfo[] drivesInfo = DriveInfo.GetDrives();

            foreach (DriveInfo driveInfo in drivesInfo)
            {
                Console.WriteLine("Drive {0}", driveInfo.Name);
                Console.WriteLine("  File type: {0}", driveInfo.DriveType);

                if (driveInfo.IsReady == true)
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
            }
        }

        static void DumpObject(object obj)
        {
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(int))
                {
                    Console.WriteLine(field.GetValue(obj));
                }
            }
        }

        /// <summary>
        /// for(initial; condition; loop)
        /// </summary>
        private static void ForLoopExample()
        {
            // for loop
            int[] values = { 1, 2, 3, 4, 5, 6 };

            for (int index = 0; index < values.Length; index++)
            {
                Console.Write(values[index]);
            }

            // A for loop with multiple loop variables
            for (int x = 0, y = values.Length - 1;
                ((x < values.Length) && (y >= 0));
                x++, y--)
            {
                Console.Write(values[x]);
                Console.Write(values[y]);
            }

            // A for loop with a custom incrementint
            for (int index = 0; index < values.Length; index += 2)
            {
                Console.Write(values[index]);
            }

            // A for loop with a break statement
            for (int index = 0; index < values.Length; index++)
            {
                if (values[index] == 4) break;
                Console.Write(values[index]);
            }

            // A for loop with a continue statement
            for (int index = 0; index < values.Length; index++)
            {
                if (values[index] == 4) continue;
                Console.Write(values[index]);
            }
        }

        private static void whileExample()
        {
            // Implementing a for loop with a while statement

            int[] values = { 1, 2, 3, 4, 5, 6 };

            {
                int index = 0;
                while (index < values.Length)
                {
                    Console.Write(values[index]);
                    index++;
                }
            }

            // do-while loop
            do
            {
                Console.WriteLine("Executed once!");
            }
            while (false);
        }

        private static void ForEachLoopExample()
        {
            int[] values = { 1, 2, 3, 4, 5, 6 };

            foreach (int i in values)
            {
                Console.Write(i);
            }
        }

        private static void GoToExample()
        {
            int x = 3;
            if (x == 3) goto customLabel;
            x++;

        customLabel:
            Console.WriteLine(x);
        }

        // Delegate Example
        public delegate int Calculate(int x, int y);

        public static int Add(int x, int y) { return x + y; }

        public static int Multiply(int x, int y) { return x * y; }

        private static void DelegatesExample()
        {
            Calculate calc = Add;
            Console.WriteLine(calc(3, 4)); // Displays 7

            calc = Multiply;
            Console.WriteLine(calc(3, 4)); // Displays 12
        }


        // multicast delegate example
        public void MethodOne()
        {
            Console.WriteLine("MethodOne");
        }

        public void MethodTwo()
        {
            Console.WriteLine("MethodTwo");
        }

        public delegate void Del();

        public void Multicast()
        {
            Del d = MethodOne;
            d += MethodTwo;

            int invocationCount = d.GetInvocationList().GetLength(0);

            d();
        }

        // Covariance with delegates
        public delegate TextWriter CovarianceDel();

        public StreamWriter MethodStream() { return null; }
        public StringWriter MethodString() { return null; }

        void DoSomething(TextWriter tw) { }
        public delegate void ContravarianceDel(StreamWriter tw);

        private void CovarianceExample()
        {
            CovarianceDel del;
            del = MethodStream;
            del = MethodString;

            ContravarianceDel del1 = DoSomething;
        }

        // End

        private static void LambdaExpressionExample()
        {
            Calculate calc = (x, y) => x + y;
            Console.WriteLine(calc(3, 4)); // Displays 7
            calc = (x, y) => x * y;
            Console.WriteLine(calc(3, 4)); // Displays 12

            Calculate calc1 =
                            (x, y) =>
                            {
                                Console.WriteLine("Adding numbers");
                                return x + y;
                            };

            int result = calc1(3, 4);
        }

        private void ActionExample()
        {
            Action<int, int> calc = (x, y) =>
            {
                Console.WriteLine(x + y);
            };

            calc(3, 4); // Displays 
        }


        static void WaitCallbackProc(Object stateInfo)
        {
            Console.WriteLine("Working on a thread from threadpool");
        }

        // symmetric encryption algorithm example

        public static void EncryptSomeText()
        {
            string original = "My secret data!";

            using (SymmetricAlgorithm symmetricAlgorithm = new AesManaged())
            {
                byte[] encrypted = Encrypt(symmetricAlgorithm, original);
                string roundtrip = Decrypt(symmetricAlgorithm, encrypted);

                // Displays: My secret data! 
                Console.WriteLine("Original:   {0}", original);
                Console.WriteLine("Round Trip: {0}", roundtrip);
            }
        }

        static byte[] Encrypt(SymmetricAlgorithm aesAlg, string plainText)
        {
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }

                    return msEncrypt.ToArray();
                }
            }
        }

        static string Decrypt(SymmetricAlgorithm aesAlg, byte[] cipherText)
        {
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        // Asymmetric encryption algorithm example
        public static void AsymmetricEncryptionExample()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string publicKeyXML = rsa.ToXmlString(false);
            string privateKeyXML = rsa.ToXmlString(true);

            Console.WriteLine(publicKeyXML);
            Console.WriteLine(privateKeyXML);

            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] dataToEncrypt = ByteConverter.GetBytes("My Secret Data!");

            byte[] encryptedData;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.FromXmlString(publicKeyXML);
                encryptedData = RSA.Encrypt(dataToEncrypt, false);
            }

            byte[] decryptedData;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.FromXmlString(privateKeyXML);
                decryptedData = RSA.Decrypt(encryptedData, false);
            }

            string decryptedString = ByteConverter.GetString(decryptedData);

            Console.WriteLine(decryptedString); // Displays: My Secret Data!

            // Using a key container for storing an asymmetric key
            string containerName = "SecretContainer";
            CspParameters csp = new CspParameters() { KeyContainerName = containerName };
            // byte[] encryptedData;

            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(csp))
            {
                encryptedData = RSA.Encrypt(dataToEncrypt, false);
            }
        }

        private static void HA256ManagedExample()
        {
            UnicodeEncoding byteConverter = new UnicodeEncoding();
            SHA256 sha256 = SHA256.Create();

            string data = "A paragraph of text";
            byte[] hashA = sha256.ComputeHash(byteConverter.GetBytes(data));

            data = "A paragraph of changed text";
            byte[] hashB = sha256.ComputeHash(byteConverter.GetBytes(data));

            data = "A paragraph of text";
            byte[] hashC = sha256.ComputeHash(byteConverter.GetBytes(data));

            Console.WriteLine(hashA.SequenceEqual(hashB)); // Displays: false
            Console.WriteLine(hashA.SequenceEqual(hashC)); // Displays: true
        }

        // Signing and verifying data with a certificate
        // makecert testCert.ce
        // makecert -n "CN=WouterDeKort" -sr currentuser -ss testCertStore

        public static void SignAndVerify()
        {
            string textToSign = "Test paragraph";
            byte[] signature = Sign(textToSign, "cn=WouterDeKort");
            // Uncomment this line to make the verification step fail
            // signature[0] = 0;
            Console.WriteLine(Verify(textToSign, signature));
        }

        static byte[] Sign(string text, string certSubject)
        {
            X509Certificate2 cert = GetCertificate();
            var csp = (RSACryptoServiceProvider)cert.PrivateKey;
            byte[] hash = HashData(text);
            return csp.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
        }

        static bool Verify(string text, byte[] signature)
        {
            X509Certificate2 cert = GetCertificate();
            var csp = (RSACryptoServiceProvider)cert.PublicKey.Key;
            byte[] hash = HashData(text);
            return csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), signature);
        }

        private static byte[] HashData(string text)
        {
            HashAlgorithm hashAlgorithm = new SHA1Managed();
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] data = encoding.GetBytes(text);
            byte[] hash = hashAlgorithm.ComputeHash(data);
            return hash;
        }

        private static X509Certificate2 GetCertificate()
        {
            X509Store my = new X509Store("testCertStore", StoreLocation.CurrentUser);
            my.Open(OpenFlags.ReadOnly);

            var certificate = my.Certificates[0];

            return certificate;
        }
        // end

        // You can specify CAS in two ways: declarative or imperative
        // Declarative CAS
        [FileIOPermission(SecurityAction.Demand, AllLocalFiles = FileIOPermissionAccess.Read)]
        public void DeclarativeCAS()
        {
            // Method body
        }

        // Imperative CAS

        public void ImperativeCAS()
        {
            FileIOPermission f = new FileIOPermission(PermissionState.None);
            f.AllLocalFiles = FileIOPermissionAccess.Read;
            try
            {
                f.Demand();
            }
            catch (SecurityException s)
            {
                Console.WriteLine(s.Message);
            }
        }

        // Initializing a SecureString
        public static void SecureStringExample()
        {
            Console.WriteLine();

            using (SecureString ss = new SecureString())
            {
                Console.Write("Please enter password: ");

                while (true)
                {
                    ConsoleKeyInfo cki = Console.ReadKey(true);
                    if (cki.Key == ConsoleKey.Enter) break;

                    ss.AppendChar(cki.KeyChar);
                    Console.Write("*");
                }

                ss.MakeReadOnly();
            }
        }


        // Getting the value of a  SecureString
        public static void ConvertToUnsecureString(SecureString securePassword)
        {
            IntPtr unmanagedString = IntPtr.Zero;

            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                Console.WriteLine(Marshal.PtrToStringUni(unmanagedString));
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        // Executing a SQL select command
        public async Task SelectDataFromTable()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProgrammingInCSharpConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM People", connection);
                await connection.OpenAsync();

                SqlDataReader dataReader = await command.ExecuteReaderAsync();

                while (await dataReader.ReadAsync())
                {
                    string formatStringWithMiddleName = "Person ({0}) is named {1} {2} {3}";
                    string formatStringWithoutMiddleName = "Person ({0}) is named {1} {3}";

                    if ((dataReader["middlename"] == null))
                    {
                        Console.WriteLine(formatStringWithoutMiddleName,
                            dataReader["id"],
                            dataReader["firstname"],
                            dataReader["lastname"]);
                    }
                    else
                    {
                        Console.WriteLine(formatStringWithMiddleName,
                            dataReader["id"],
                            dataReader["firstname"],
                            dataReader["middlename"],
                            dataReader["lastname"]);
                    }
                }
                dataReader.Close();
            }
        }

        public async Task SelectMultipleResultSets()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProgrammingInCSharpConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM People; SELECT TOP 1 * FROM People ORDER BY LastName", connection);

                await connection.OpenAsync();
                SqlDataReader dataReader = await command.ExecuteReaderAsync();
                await ReadQueryResults(dataReader);
                await dataReader.NextResultAsync(); // Move to the next result set
                await ReadQueryResults(dataReader);
                dataReader.Close();
            }
        }
        private static async Task ReadQueryResults(SqlDataReader dataReader)
        {
            while (await dataReader.ReadAsync())
            {
                string formatStringWithMiddleName = "Person ({0}) is named {1} {2} {3}";
                string formatStringWithoutMiddleName = "Person ({0}) is named {1} {3}";
                if ((dataReader["middlename"] == null))
                {
                    Console.WriteLine(formatStringWithoutMiddleName,
                        dataReader["id"],
                        dataReader["firstname"],
                        dataReader["lastname"]);
                }
                else
                {
                    Console.WriteLine(formatStringWithMiddleName,
                        dataReader["id"],
                        dataReader["firstname"],
                        dataReader["middlename"],
                        dataReader["lastname"]);
                }
            }
        }

        public async Task UpdateRows()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProgrammingInCSharpConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("UPDATE People SET FirstName='John'", connection);

                await connection.OpenAsync();
                int numberOfUpdatedRows = await command.ExecuteNonQueryAsync();
                Console.WriteLine("Updated {0} rows", numberOfUpdatedRows);
            }
        }

        public async Task InsertRowWithParameterizedQuery()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProgrammingInCSharpConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO People([FirstName], [LastName], [MiddleName]) VALUES(@firstName, @lastName, @middleName)", connection);
                await connection.OpenAsync();

                command.Parameters.AddWithValue("@firstName", "John");
                command.Parameters.AddWithValue("@lastName", "Doe");
                command.Parameters.AddWithValue("@middleName", "Little");

                int numberOfInsertedRows = await command.ExecuteNonQueryAsync();
                Console.WriteLine("Inserted {0} rows", numberOfInsertedRows);
            }
        }

        private void TransactionScopeExample()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProgrammingInCSharpConnection"].ConnectionString;

            using (TransactionScope transactionScope = new TransactionScope())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command1 = new SqlCommand("INSERT INTO People ([FirstName], [LastName], [MiddleInitial]) VALUES('John', 'Doe', null)", connection);
                    SqlCommand command2 = new SqlCommand("INSERT INTO People ([FirstName], [LastName], [MiddleInitial]) VALUES('Jane', 'Doe', null)", connection);

                    command1.ExecuteNonQuery();
                    command2.ExecuteNonQuery();
                }
                transactionScope.Complete();
            }
        }


        // This function takes arguments for 2 connection strings and commands to create a transaction 
        // involving two SQL Servers. It returns a value > 0 if the transaction is committed, 0 if the 
        // transaction is rolled back. To test this code, you can connect to two different databases 
        // on the same server by altering the connection string, or to another 3rd party RDBMS by 
        // altering the code in the connection2 code block.
        static public int CreateTransactionScope(
            string connectString1, string connectString2,
            string commandText1, string commandText2)
        {
            // Initialize the return value to zero and create a StringWriter to display results.
            int returnValue = 0;
            System.IO.StringWriter writer = new System.IO.StringWriter();

            try
            {
                // Create the TransactionScope to execute the commands, guaranteeing
                // that both commands can commit or roll back as a single unit of work.
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection connection1 = new SqlConnection(connectString1))
                    {
                        // Opening the connection automatically enlists it in the 
                        // TransactionScope as a lightweight transaction.
                        connection1.Open();

                        // Create the SqlCommand object and execute the first command.
                        SqlCommand command1 = new SqlCommand(commandText1, connection1);
                        returnValue = command1.ExecuteNonQuery();
                        writer.WriteLine("Rows to be affected by command1: {0}", returnValue);

                        // If you get here, this means that command1 succeeded. By nesting
                        // the using block for connection2 inside that of connection1, you
                        // conserve server and network resources as connection2 is opened
                        // only when there is a chance that the transaction can commit.   
                        using (SqlConnection connection2 = new SqlConnection(connectString2))
                        {
                            // The transaction is escalated to a full distributed
                            // transaction when connection2 is opened.
                            connection2.Open();

                            // Execute the second command in the second database.
                            returnValue = 0;
                            SqlCommand command2 = new SqlCommand(commandText2, connection2);
                            returnValue = command2.ExecuteNonQuery();
                            writer.WriteLine("Rows to be affected by command2: {0}", returnValue);
                        }
                    }

                    // The Complete method commits the transaction. If an exception has been thrown,
                    // Complete is not  called and the transaction is rolled back.
                    scope.Complete();

                }

            }
            catch (TransactionAbortedException ex)
            {
                writer.WriteLine("TransactionAbortedException Message: {0}", ex.Message);
            }
            catch (ApplicationException ex)
            {
                writer.WriteLine("ApplicationException Message: {0}", ex.Message);
            }

            // Display messages.
            Console.WriteLine(writer.ToString());

            return returnValue;
        }

        [Conditional("DEBUG")]
        private static void Log(string message)
        {
            Console.WriteLine("message");
        }

        // Building a directory tree
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

        // Executing multiple awaits
        public async Task ExecuteMultipleRequests()
        {
            HttpClient client = new HttpClient();

            string microsoft = await client.GetStringAsync("http://www.microsoft.com");
            string msdn = await client.GetStringAsync("http://msdn.microsoft.com");
            string blogs = await client.GetStringAsync("http://blogs.msdn.com/");
        }

        // Executing multiple requests in parallel
        public async Task ExecuteMultipleRequestsInParallel()
        {
            HttpClient client = new HttpClient();

            Task microsoft = client.GetStringAsync("http://www.microsoft.com");
            Task msdn = client.GetStringAsync("http://msdn.microsoft.com");
            Task blogs = client.GetStringAsync("http://blogs.msdn.com/");

            await Task.WhenAll(microsoft, msdn, blogs);
        }

        // Using preprocessor directives to target multiple platforms
        public Assembly LoadAssembly<T>()
        {
#if !WINRT
            Assembly assembly = typeof(T).Assembly;
#else
                    Assembly assembly = typeof(T).GetTypeInfo().Assembly;
#endif

            return assembly;
        }

        public static bool IsEven(int i)
        {
            if (i % 10 == 0) throw new ArgumentException("i");
            return i % 2 == 0;
        }

        public static void ThreadMethod()
        {
            for (int i = 0; i < 10; i++)
            {
                System.Console.WriteLine("Thread Proc: {0}", i);
                Thread.Sleep(100);
            }
        }

        public static void ThreadMethodWithParameter(object data)
        {
            System.Console.WriteLine("Thread parameter value: {0}", data);

            for (int i = 0; i < (int)data; i++)
            {
                System.Console.WriteLine("Thread Proc with parameter: {0}", i);
                Thread.Sleep(100);
            }
        }

        public static async Task<string> DownloadContent()
        {
            using (HttpClient client = new HttpClient())
            {
                string result = await client.GetStringAsync("http://www.tahmilur.com");
                return result;
            }
        }

        public static async Task<string> GetSiteData()
        {
            using (HttpClient client = new HttpClient())
            {
                string result = await client.GetStringAsync("http://www.tahmilur.com");
                return result;
            }
        }

        // Scalability versus responsiveness
        public Task SleepAsyncA(int millisecondsTimeout)
        {
            return Task.Run(() => Thread.Sleep(millisecondsTimeout));
        }

        public Task SleepAsyncB(int millisecondsTimeout)
        {
            TaskCompletionSource<bool> tcs = null;
            var t = new Timer(delegate { tcs.TrySetResult(true); }, null, -1, -1);
            tcs = new TaskCompletionSource<bool>(t);
            t.Change(millisecondsTimeout, -1);
            return tcs.Task;
        }

        // Using ConfigureAwait
        private async void ConfigureAwaitExample()
        {
            HttpClient httpClient = new HttpClient();

            string content = await httpClient.GetStringAsync("http://www.microsoft.com")
                .ConfigureAwait(false);

            // Output.Content = content;

            using (FileStream sourceStream = new FileStream("temp.html",
            FileMode.Create, FileAccess.Write, FileShare.None,
            4096, useAsync: true))
            {
                byte[] encodedText = Encoding.Unicode.GetBytes(content);
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length)
                    .ConfigureAwait(false);
            };
        }
    }
}