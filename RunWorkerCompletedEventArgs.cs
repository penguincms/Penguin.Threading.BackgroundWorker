using System;

namespace Penguin.Threading.BackgroundWorker
{
    public sealed class RunWorkerCompletedEventArgs<T> : EventArgs
    {
        public bool Cancelled
        {
            get;
            private set;
        }

        public Exception Error
        {
            get;
            private set;
        }

        public T Result
        {
            get;
            private set;
        }

        public RunWorkerCompletedEventArgs(T result, Exception error, bool cancelled)
        {
            Result = result;
            Error = error;
            Cancelled = cancelled;
        }
    }
}