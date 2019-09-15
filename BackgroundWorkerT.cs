using System;
using System.ComponentModel;

namespace Penguin.Threading
{
    public class BackgroundWorker<TArgument> : BackgroundWorker
    {
        public Action<BackgroundWorker<TArgument>, TArgument> DoWork;
        public BackgroundWorker() : base()
        {
            InternalWorker.DoWork += this.InternalWorker_DoWork;
        }

        public bool RunWorkerAsync(TArgument argument)
        {
            if (InternalWorker.IsBusy)
            {
                return false;
            }
            else
            {
                InternalWorker.RunWorkerAsync(argument);
                return true;
            }
        }

        private void InternalWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            DoWork.Invoke(this, (TArgument)e.Argument);
        }
    }
}