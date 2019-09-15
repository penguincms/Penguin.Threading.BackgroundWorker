using System.ComponentModel;

namespace Penguin.Threading
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