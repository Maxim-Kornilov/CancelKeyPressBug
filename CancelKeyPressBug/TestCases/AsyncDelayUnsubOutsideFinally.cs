using System;
using System.Threading.Tasks;

namespace CancelKeyPressBug.TestCases
{
    class AsyncDelayUnsubOutsideFinally : ITestCase
    {
        public void Run() => RunAsync().GetAwaiter().GetResult();

        public async Task RunAsync()
        {
            var tokenSource = new CancelKeyPressedTokenSource();

            try
            {
                await Task.Delay(TimeSpan.FromHours(1.0), tokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Cancelled");
            }
            tokenSource.Dispose();
        }
    }
}
