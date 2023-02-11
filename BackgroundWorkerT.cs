using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Penguin.Threading.BackgroundWorker
{
    public class BackgroundWorker<TArgument> : AbstractBackgroundWorker
    {
        public Action<BackgroundWorker<TArgument>, TArgument> DoWork { get; set; }

        protected TaskCompletionSource<bool> ResultTaskSource { get; set; }

        public BackgroundWorker() : base()
        {
            ResultTaskSource = new TaskCompletionSource<bool>();
            InternalWorker.DoWork += InternalWorker_DoWork;
            InternalWorker.RunWorkerCompleted += InternalWorker_RunWorkerCompleted;
        }

        public Task<bool> RunWorkerAsync(TArgument argument)
        {
            if (InternalWorker.IsBusy)
            {
                _ = ResultTaskSource.TrySetResult(false);
            }
            else
            {
                InternalWorker.RunWorkerAsync(argument);
            }

            return ResultTaskSource.Task;
        }

        private void InternalWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            DoWork.Invoke(this, (TArgument)e.Argument);
        }

        private void InternalWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _ = ResultTaskSource.TrySetResult(true);
        }
    }
}