﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace CancelKeyPressBug.TestCases
{
    class AsyncDelaySubAndUnsubAfterCancel : ITestCase
    {
        public void Run() => RunAsync().GetAwaiter().GetResult();

        public async Task RunAsync()
        {
            var tokenSource = new CancellationTokenSource();

            try
            {
                await Task.Delay(TimeSpan.FromHours(1.0), tokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Cancelled");
            }

            var consoleTokenSource = new CancelKeyPressedTokenSource();
            consoleTokenSource.Dispose();
        }
    }
}
