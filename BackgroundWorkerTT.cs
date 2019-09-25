using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Penguin.Threading
{
    public class BackgroundWorker<TArgument, TResult> : AbstractBackgroundWorker
    {
        protected TaskCompletionSource<TResult> ResultTaskSource { get; set; }
        public Func<BackgroundWorker<TArgument, TResult>, TArgument, TResult> DoWork;

        public BackgroundWorker() : base()
        {
            ResultTaskSource = new TaskCompletionSource<TResult>();
            InternalWorker.DoWork += this.InternalWorker_DoWork;
            InternalWorker.RunWorkerCompleted += InternalWorker_RunWorkerCompleted;
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

        private void InternalWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ResultTaskSource.TrySetResult((TResult)e.Result);
        }
    }
}