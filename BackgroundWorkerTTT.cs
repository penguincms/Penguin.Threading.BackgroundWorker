using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Penguin.Threading
{
    public class BackgroundWorker<TArgument, TProgress, TResult> : AbstractBackgroundWorker
    {
        public Func<BackgroundWorker<TArgument, TProgress, TResult>, TArgument, TResult> DoWork { get; set; }
        public Action<BackgroundWorker<TArgument, TProgress, TResult>, ProgressChangedEventArgs<TProgress>> ProgressChanged { get; set; }
        protected TaskCompletionSource<TResult> ResultTaskSource { get; set; }

        public BackgroundWorker() : base()
        {
            this.InternalWorker.DoWork += this.InternalWorker_DoWork;
            this.InternalWorker.ProgressChanged += this.InternalWorker_ProgressChanged;
            this.InternalWorker.RunWorkerCompleted += this.InternalWorker_RunWorkerCompleted;
        }

        public void ReportProgress(int percentProgress)
        {
            this.InternalWorker.ReportProgress(percentProgress);
        }

        public void ReportProgress(int percentProgress, ProgressChangedEventArgs<TProgress> userState)
        {
            this.InternalWorker.ReportProgress(percentProgress, userState);
        }

        public Task<TResult> RunWorkerAsync(TArgument argument)
        {
            if (this.InternalWorker.IsBusy)
            {
                return null;
            }
            else
            {
                this.InternalWorker.RunWorkerAsync(argument);
            }

            return this.ResultTaskSource.Task;
        }

        private void InternalWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            TResult result = this.DoWork.Invoke(this, (TArgument)e.Argument);

            e.Result = result;
        }

        private void InternalWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.ProgressChanged?.Invoke(this, new ProgressChangedEventArgs<TProgress>(e.ProgressPercentage, (TProgress)e.UserState));
        }

        private void InternalWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ResultTaskSource.TrySetResult((TResult)e.Result);
        }
    }
}