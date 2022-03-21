using System;
using System.Threading.Tasks;

namespace Penguin.Threading
{
    public class BackgroundWorker : AbstractBackgroundWorker
    {
        public Action<BackgroundWorker> DoWork { get; set; }
        protected TaskCompletionSource<bool> ResultTaskSource { get; set; }

        public BackgroundWorker() : base()
        {
            this.ResultTaskSource = new TaskCompletionSource<bool>();
            this.InternalWorker.DoWork += this.InternalWorker_DoWork;
            this.InternalWorker.RunWorkerCompleted += this.InternalWorker_RunWorkerCompleted;
        }

        public static BackgroundWorker Create(Action<BackgroundWorker> doWork)
        {
            BackgroundWorker toReturn = new BackgroundWorker
            {
                DoWork = doWork
            };

            return toReturn;
        }

        public static BackgroundWorker<TArgument> Create<TArgument>(Action<BackgroundWorker<TArgument>, TArgument> doWork)
        {
            BackgroundWorker<TArgument> toReturn = new BackgroundWorker<TArgument>
            {
                DoWork = doWork
            };

            return toReturn;
        }

        public static BackgroundWorker<TArgument, TResult> Create<TArgument, TResult>(Func<BackgroundWorker<TArgument, TResult>, TArgument, TResult> doWork)
        {
            BackgroundWorker<TArgument, TResult> toReturn = new BackgroundWorker<TArgument, TResult>
            {
                DoWork = doWork
            };

            return toReturn;
        }

        public static BackgroundWorker<TArgument, TProgress, TResult> Create<TArgument, TProgress, TResult>(Func<BackgroundWorker<TArgument, TProgress, TResult>, TArgument, TResult> doWork, Action<BackgroundWorker<TArgument, TProgress, TResult>, ProgressChangedEventArgs<TProgress>> progressChanged)
        {
            BackgroundWorker<TArgument, TProgress, TResult> toReturn = new BackgroundWorker<TArgument, TProgress, TResult>
            {
                DoWork = doWork,

                ProgressChanged = progressChanged
            };

            return toReturn;
        }

        public Task<bool> RunWorkerAsync()
        {
            if (this.InternalWorker.IsBusy)
            {
                this.ResultTaskSource.TrySetResult(false);
            }
            else
            {
                this.InternalWorker.RunWorkerAsync();
            }

            return this.ResultTaskSource.Task;
        }

        private void InternalWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) => this.DoWork.Invoke(this);

        private void InternalWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e) => this.ResultTaskSource.TrySetResult(true);
    }
}