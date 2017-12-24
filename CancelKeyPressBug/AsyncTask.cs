using System.Threading;
using System.Threading.Tasks;

namespace CancelKeyPressBug
{
    static class AsyncTask
    {
        public static Task Run(CancellationToken token)
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                }
            }, token);
        }
    }
}
