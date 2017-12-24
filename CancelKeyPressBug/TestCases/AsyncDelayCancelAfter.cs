using System;
using System.Threading;
using System.Threading.Tasks;

namespace CancelKeyPressBug.TestCases
{
    class AsyncDelayCancelAfter : ITestCase
    {
        public void Run() => RunAsync().GetAwaiter().GetResult();

        public async Task RunAsync()
        {
            var tokenSource = new CancellationTokenSource();

            ConsoleCancelEventHandler handler = (s, e) =>
            {
                e.Cancel = true;
                Console.WriteLine("Cancel after 2 second");
                // Violate condition #3
                tokenSource.CancelAfter(TimeSpan.FromSeconds(2.0));
            };
            Console.CancelKeyPress += handler;

            try
            {
                await Task.Delay(TimeSpan.FromHours(1.0), tokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Cancelled");
            }

            Console.CancelKeyPress -= handler;
        }
    }
}
