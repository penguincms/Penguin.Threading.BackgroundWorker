using System;
using System.Threading.Tasks;

namespace Penguin.Threading.BackgroundWorker
{
    public class BackgroundWorker : AbstractBackgroundWorker
    {
        public Action<BackgroundWorker> DoWork { get; set; }

        protected TaskCompletionSource<bool> ResultTaskSource { get; set; }

        public BackgroundWorker() : base()
        {
            ResultTaskSource = new TaskCompletionSource<bool>();
            InternalWorker.DoWork += InternalWorker_DoWork;
            InternalWorker.RunWorkerCompleted += InternalWorker_RunWorkerCompleted;
        }

        public static BackgroundWorker Create(Action<BackgroundWorker> doWork)
        {
            BackgroundWorker toReturn = new()
            {
                DoWork = doWork
            };

            return toReturn;
        }

        public static BackgroundWorker<TArgument> Create<TArgument>(Action<BackgroundWorker<TArgument>, TArgument> doWork)
        {
            BackgroundWorker<TArgument> toReturn = new()
            {
                DoWork = doWork
            };

            return toReturn;
        }

        public static BackgroundWorker<TArgument, TResult> Create<TArgument, TResult>(Func<BackgroundWorker<TArgument, TResult>, TArgument, TResult> doWork)
        {
            BackgroundWorker<TArgument, TResult> toReturn = new()
            {
                DoWork = doWork
            };

            return toReturn;
        }

        public static BackgroundWorker<TArgument, TProgress, TResult> Create<TArgument, TProgress, TResult>(Func<BackgroundWorker<TArgument, TProgress, TResult>, TArgument, TResult> doWork, Action<BackgroundWorker<TArgument, TProgress, TResult>, ProgressChangedEventArgs<TProgress>> progressChanged)
        {
            BackgroundWorker<TArgument, TProgress, TResult> toReturn = new()
            {
                DoWork = doWork,

                ProgressChanged = progressChanged
            };

            return toReturn;
        }

        public Task<bool> RunWorkerAsync()
        {
            if (InternalWorker.IsBusy)
            {
                _ = ResultTaskSource.TrySetResult(false);
            }
            else
            {
                InternalWorker.RunWorkerAsync();
            }

            return ResultTaskSource.Task;
        }

        private void InternalWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            DoWork.Invoke(this);
        }

        private void InternalWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            _ = ResultTaskSource.TrySetResult(true);
        }
    }
}