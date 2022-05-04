using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Penguin.Threading.BackgroundWorker
{
    public class BackgroundWorker<TArgument, TResult> : AbstractBackgroundWorker
    {
        public Func<BackgroundWorker<TArgument, TResult>, TArgument, TResult> DoWork { get; set; }
        protected TaskCompletionSource<TResult> ResultTaskSource { get; set; }

        public BackgroundWorker() : base()
        {
            this.ResultTaskSource = new TaskCompletionSource<TResult>();
            this.InternalWorker.DoWork += this.InternalWorker_DoWork;
            this.InternalWorker.RunWorkerCompleted += this.InternalWorker_RunWorkerCompleted;
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

        private void InternalWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _ = this.ResultTaskSource.TrySetResult((TResult)e.Result);
        }
    }
}