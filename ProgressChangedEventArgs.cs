using System;

namespace Penguin.Threading
{
    public class ProgressChangedEventArgs<T> : EventArgs
    {
        public int ProgressPercentage
        {
            get;
            private set;
        }

        public T UserState
        {
            get;
            private set;
        }

        public ProgressChangedEventArgs(int progressPercentage, T userState)
        {
            ProgressPercentage = progressPercentage;
            UserState = userState;
        }
    }
}