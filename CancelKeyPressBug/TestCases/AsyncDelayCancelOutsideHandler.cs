using System;
using System.Threading;
using System.Threading.Tasks;

namespace CancelKeyPressBug.TestCases
{
    class AsyncDelayCancelOutsideHandler : ITestCase
    {
        public void Run() => RunAsync().GetAwaiter().GetResult();

        public async Task RunAsync()
        {
            var executor = new SingleThreadExecutor();
            var tokenSource = new CancellationTokenSource();

            ConsoleCancelEventHandler handler = (s, e) =>
            {
                e.Cancel = true;
                // Violate condition #2
                Task.Run(() => tokenSource.Cancel());
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
