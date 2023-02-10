using System.ComponentModel;

namespace Penguin.Threading.BackgroundWorker
{
    public class DoWorkEventArgs<TArgument> : CancelEventArgs
    {
        public TArgument Argument
        {
            get;
            private set;
        }

        public DoWorkEventArgs(TArgument argument)
        {
            Argument = argument;
        }
    }
}