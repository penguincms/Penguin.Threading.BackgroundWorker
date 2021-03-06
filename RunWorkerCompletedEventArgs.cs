﻿using System;

namespace Penguin.Threading
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
            this.Result = result;
            this.Error = error;
            this.Cancelled = cancelled;
        }
    }
}