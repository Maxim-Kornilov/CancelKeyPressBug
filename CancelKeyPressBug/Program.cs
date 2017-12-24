using CancelKeyPressBug.TestCases;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace CancelKeyPressBug
{
    class Program
    {
        private static Type[] TestCases = new Type[]
        {
            typeof(AsyncDelayCancel), // Hangs
            typeof(AsyncDelayCancelAfter),
            typeof(AsyncDelayNoCalcel),
            typeof(AsyncDelayNoCancelThrow),
            typeof(AsyncDelaySubAndUnsubAfterCancel),
            typeof(AsyncDelayUnsubOutsideFinally), // Hung
            typeof(AsyncDelayUnsubSameThread), // Hangs
            typeof(AsyncDelayUnsubSameThreadOutsideFinally), // Hang
            typeof(AsyncTaskCancel),
            typeof(SyncDelay),
        };

        private static void Main(string[] args)
        {
            RunTestCases(args);

            Console.Write("Application will close in: ");
            for (int i = 2; i > 0; i--)
            {
                Console.Write(i);
                Console.CursorLeft--;
                Thread.Sleep(1000);
            }
            Console.Write("0");
        }

        private static void RunTestCases(string[] args)
        {
            if (args.Length == 0)
            {
                RunAll();
            }
            else if (Int32.TryParse(args[0], out int index))
            {
                Run(index);
            }
            else
            {
                Console.WriteLine("Invalid test case index");
            }
        }

        private static void Run(int index)
        {
            var testCaseType = TestCases[index];
            Console.WriteLine("Test case: " + testCaseType.Name);

            var testCase = (ITestCase)Activator.CreateInstance(testCaseType);

            Console.WriteLine("Press Ctrl+C to continue");
            testCase.Run();
        }

        private static void RunAll()
        {
            string appPath = Assembly.GetExecutingAssembly().Location;

            for (int i = 0; i < TestCases.Length; i++)
            {
                Console.WriteLine("Start " + TestCases[i].Name);
                var process = new Process();
                process.StartInfo.FileName = "dotnet";
                process.StartInfo.Arguments = $"\"{appPath}\" {i}";
                process.StartInfo.UseShellExecute = true;
                process.Start();
            }
        }
    }
}
