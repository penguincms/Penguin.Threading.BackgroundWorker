using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Penguin.Threading
{
    public class BackgroundWorker<TArgument> : AbstractBackgroundWorker
    {
        protected TaskCompletionSource<bool> ResultTaskSource { get; set; }
        public Action<BackgroundWorker<TArgument>, TArgument> DoWork;

        public BackgroundWorker() : base()
        {
            ResultTaskSource = new TaskCompletionSource<bool>();
            InternalWorker.DoWork += this.InternalWorker_DoWork;
            InternalWorker.RunWorkerCompleted += this.InternalWorker_RunWorkerCompleted;
        }

        public Task<bool> RunWorkerAsync(TArgument argument)
        {
            if (InternalWorker.IsBusy)
            {
                ResultTaskSource.TrySetResult(false);
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
            ResultTaskSource.TrySetResult(true);
        }
    }
}