using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Penguin.Threading.BackgroundWorker
{
    public class BackgroundWorker<TArgument, TProgress, TResult> : AbstractBackgroundWorker
    {
        public Func<BackgroundWorker<TArgument, TProgress, TResult>, TArgument, TResult> DoWork { get; set; }

        public Action<BackgroundWorker<TArgument, TProgress, TResult>, ProgressChangedEventArgs<TProgress>> ProgressChanged { get; set; }

        protected TaskCompletionSource<TResult> ResultTaskSource { get; set; }

        public BackgroundWorker() : base()
        {
            InternalWorker.DoWork += InternalWorker_DoWork;
            InternalWorker.ProgressChanged += InternalWorker_ProgressChanged;
            InternalWorker.RunWorkerCompleted += InternalWorker_RunWorkerCompleted;
        }

        public void ReportProgress(int percentProgress)
        {
            InternalWorker.ReportProgress(percentProgress);
        }

        public void ReportProgress(int percentProgress, ProgressChangedEventArgs<TProgress> userState)
        {
            InternalWorker.ReportProgress(percentProgress, userState);
        }

        public Task<TResult> RunWorkerAsync(TArgument argument)
        {
            if (InternalWorker.IsBusy)
            {
                return null;
            }
            else
            {
                InternalWorker.RunWorkerAsync(argument);
            }

            return ResultTaskSource.Task;
        }

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
            _ = ResultTaskSource.TrySetResult((TResult)e.Result);
        }
    }
}