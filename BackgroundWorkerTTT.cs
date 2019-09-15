using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Penguin.Threading
{
    public class BackgroundWorker<TArgument, TProgress, TResult> : BackgroundWorkerBase
    {
        public Func<BackgroundWorker<TArgument, TProgress, TResult>, TArgument, TResult> DoWork;

        public Action<BackgroundWorker<TArgument, TProgress, TResult>, ProgressChangedEventArgs<TProgress>> ProgressChanged;

        public BackgroundWorker() : base()
        {
            InternalWorker.DoWork += this.InternalWorker_DoWork;
            InternalWorker.ProgressChanged += this.InternalWorker_ProgressChanged;
        }

        public void ReportProgress(int percentProgress) => InternalWorker.ReportProgress(percentProgress);

        public void ReportProgress(int percentProgress, ProgressChangedEventArgs<TProgress> userState) => InternalWorker.ReportProgress(percentProgress, userState);

        public Task<TResult> RunWorkerAsync(TArgument argument)
        {
            if (InternalWorker.IsBusy)
            {
                return null;
            }
            else
            {
                InternalWorker.RunWorkerAsync(argument);
                return ResultTaskSource.Task;
            }
        }

        protected TaskCompletionSource<TResult> ResultTaskSource { get; set; }

        private void InternalWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            TResult result = DoWork.Invoke(this, (TArgument)e.Argument);

            e.Result = result;
        }

        private void InternalWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressChanged?.Invoke(this, new ProgressChangedEventArgs<TProgress>(e.ProgressPercentage, (TProgress)e.UserState));
        }

        private void InternalWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ResultTaskSource.TrySetResult((TResult)e.Result);
        }
    }
}