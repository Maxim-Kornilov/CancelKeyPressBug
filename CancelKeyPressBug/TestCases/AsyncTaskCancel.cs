using System;
using System.Threading.Tasks;

namespace CancelKeyPressBug.TestCases
{
    class AsyncTaskCancel : ITestCase
    {
        public void Run() => RunAsync().GetAwaiter().GetResult();

        public async Task RunAsync()
        {
            var tokenSource = new CancelKeyPressedTokenSource();

            try
            {
                await AsyncTask.Run(tokenSource.Token);
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
