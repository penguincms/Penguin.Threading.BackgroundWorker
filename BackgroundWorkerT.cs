using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Penguin.Threading
{
    public class BackgroundWorker<TArgument> : AbstractBackgroundWorker
    {
        public Action<BackgroundWorker<TArgument>, TArgument> DoWork { get; set; }
        protected TaskCompletionSource<bool> ResultTaskSource { get; set; }

        public BackgroundWorker() : base()
        {
            this.ResultTaskSource = new TaskCompletionSource<bool>();
            this.InternalWorker.DoWork += this.InternalWorker_DoWork;
            this.InternalWorker.RunWorkerCompleted += this.InternalWorker_RunWorkerCompleted;
        }

        public Task<bool> RunWorkerAsync(TArgument argument)
        {
            if (this.InternalWorker.IsBusy)
            {
                this.ResultTaskSource.TrySetResult(false);
            }
            else
            {
                this.InternalWorker.RunWorkerAsync(argument);
            }

            return this.ResultTaskSource.Task;
        }

        private void InternalWorker_DoWork(object sender, DoWorkEventArgs e) => this.DoWork.Invoke(this, (TArgument)e.Argument);

        private void InternalWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) => this.ResultTaskSource.TrySetResult(true);
    }
}