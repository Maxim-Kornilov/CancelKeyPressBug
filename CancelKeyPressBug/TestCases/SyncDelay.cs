using System;
using System.Threading.Tasks;

namespace CancelKeyPressBug.TestCases
{
    class SyncDelay : ITestCase
    {
        public void Run()
        {
            var tokenSource = new CancelKeyPressedTokenSource();

            try
            {
                Task.Delay(TimeSpan.FromHours(1.0), tokenSource.Token).GetAwaiter().GetResult();
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
