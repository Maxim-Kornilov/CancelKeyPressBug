using System;
using System.Threading;

namespace CancelKeyPressBug
{
    class CancelKeyPressedTokenSource : IDisposable
    {
        private CancellationTokenSource _tokenSource;
        private bool _isDisposed;

        public CancellationToken Token => _tokenSource.Token;
        public bool IsCancellationRequested => _tokenSource.IsCancellationRequested;

        public CancelKeyPressedTokenSource()
        {
            _tokenSource = new CancellationTokenSource();
            _isDisposed = false;
            Console.WriteLine("Subscribe");
            Console.CancelKeyPress += Cancelled;
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            Console.WriteLine("Unsubscribe");
            Console.CancelKeyPress -= Cancelled;
            Console.WriteLine("Unsubscribed");
            _tokenSource.Dispose();
        }

        private void Cancelled(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Cancel");
            _tokenSource.Cancel();
            e.Cancel = true;
        }
    }
}
