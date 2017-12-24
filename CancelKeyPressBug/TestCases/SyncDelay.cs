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
                // Violate condition #4
                Task.Delay(TimeSpan.FromHours(1.0), tokenSource.Token).GetAwaiter().GetResult();
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Cancelled");
            }

            tokenSource.Dispose();
        }
    }
}
