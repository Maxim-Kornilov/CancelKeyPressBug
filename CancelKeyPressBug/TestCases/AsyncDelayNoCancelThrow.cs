using System;
using System.Threading.Tasks;

namespace CancelKeyPressBug.TestCases
{
    class AsyncDelayNoCancelThrow : ITestCase
    {
        public void Run() => RunAsync().GetAwaiter().GetResult();

        public async Task RunAsync()
        {
            var tokenSource = new CancelKeyPressedTokenSource();

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(1.0));
                throw new OperationCanceledException(tokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Cancelled");
            }
            finally
            {
                tokenSource.Dispose();
            }
        }
    }
}
