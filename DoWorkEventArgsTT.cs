using System.ComponentModel;

namespace Penguin.Threading.BackgroundWorker
{
    public class DoWorkEventArgs<TArgument, TResult> : CancelEventArgs
    {
        public TArgument Argument
        {
            get;
            private set;
        }

        public TResult Result
        {
            get;
            set;
        }

        public DoWorkEventArgs(TArgument argument)
        {
            this.Argument = argument;
        }
    }
}