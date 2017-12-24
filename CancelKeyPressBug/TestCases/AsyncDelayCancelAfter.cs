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
            ConsoleCancelEventHandler handler = (s, e) => 
            {
                e.Cancel = true;
                Console.WriteLine("Ctrl+C is pressed");
            };
            Console.CancelKeyPress += handler;

            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(TimeSpan.FromSeconds(5.0));

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
