using System;

namespace Penguin.Threading
{
    public class BackgroundWorkerBase
    {
        public bool CancellationPending { get; protected set; }
        public bool IsBusy => InternalWorker.IsBusy;

        public BackgroundWorkerBase()
        {
            InternalWorker = new System.ComponentModel.BackgroundWorker();
        }

        public void CancelAsync()
        {
            CancellationPending = true;
        }

        protected System.ComponentModel.BackgroundWorker InternalWorker { get; set; }

        public static BackgroundWorker<TArgument> Create<TArgument>(Action<BackgroundWorker<TArgument>, TArgument> doWork)
        {
            BackgroundWorker<TArgument> toReturn = new BackgroundWorker<TArgument>();

            toReturn.DoWork = doWork;

            return toReturn;
        }

        public static BackgroundWorker<TArgument, TResult> Create<TArgument, TResult>(Func<BackgroundWorker<TArgument, TResult>, TArgument, TResult> doWork)
        {
            BackgroundWorker<TArgument, TResult> toReturn = new BackgroundWorker<TArgument, TResult>();

            toReturn.DoWork = doWork;

            return toReturn;
        }

        public static BackgroundWorker<TArgument, TProgress, TResult> Create<TArgument, TProgress, TResult>(Func<BackgroundWorker<TArgument, TProgress, TResult>, TArgument, TResult> doWork, Action<BackgroundWorker<TArgument, TProgress, TResult>, ProgressChangedEventArgs<TProgress>> progressChanged)
        {
            BackgroundWorker<TArgument, TProgress, TResult> toReturn = new BackgroundWorker<TArgument, TProgress, TResult>();

            toReturn.DoWork = doWork;

            toReturn.ProgressChanged = progressChanged;

            return toReturn;
        }
    }
}