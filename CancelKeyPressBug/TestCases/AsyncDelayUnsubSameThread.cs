using System;
using System.Threading.Tasks;

namespace CancelKeyPressBug.TestCases
{
    class AsyncDelayUnsubSameThread : ITestCase
    {
        public void Run() => RunAsync().GetAwaiter().GetResult();

        public async Task RunAsync()
        {
            var executor = new SingleThreadExecutor();

            var tokenSource = executor.Execute(() => new CancelKeyPressedTokenSource());

            try
            {
                await Task.Delay(TimeSpan.FromHours(1.0), tokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Cancelled");
            }
            finally
            {
                executor.Execute(() => tokenSource.Dispose());
            }
        }
    }
}
