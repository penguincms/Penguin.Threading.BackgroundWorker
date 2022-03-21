namespace Penguin.Threading
{
    public abstract class AbstractBackgroundWorker
    {
        public bool CancellationPending { get; protected set; }
        public bool IsBusy => this.InternalWorker.IsBusy;

        protected System.ComponentModel.BackgroundWorker InternalWorker { get; set; }

        public AbstractBackgroundWorker()
        {
            this.InternalWorker = new System.ComponentModel.BackgroundWorker();
        }

        public void CancelAsync() => this.CancellationPending = true;
    }
}