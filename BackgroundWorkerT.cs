
/* Unmerged change from project 'Penguin.Threading.BackgroundWorker.Local (net5.0)'
Before:
using System;
After:
using Penguin.Threading.BackgroundWorker;
using Penguin.Threading.BackgroundWorker.BackgroundWorker;
using System;
*/

/* Unmerged change from project 'Penguin.Threading.BackgroundWorker.Local (netstandard2.1)'
Before:
using System;
After:
using Penguin.Threading.BackgroundWorker;
using System;
*/

/* Unmerged change from project 'Penguin.Threading.BackgroundWorker.Local (net5.0)'
Before:
using Penguin.Threading.BackgroundWorker.BackgroundWorker;
After:
using Penguin;
using Penguin.Threading;
using Penguin.Threading.BackgroundWorker;
using Penguin.Threading.BackgroundWorker.BackgroundWorker;
*/

/* Unmerged change from project 'Penguin.Threading.BackgroundWorker.Local (netstandard2.1)'
Before:
using Penguin.Threading.BackgroundWorker.BackgroundWorker;
After:
using Penguin;
using Penguin.Threading;
using Penguin.Threading.BackgroundWorker;
using Penguin.Threading.BackgroundWorker.BackgroundWorker;
*/
using System;
using System.ComponentModel;
using System.Threading.Tasks;


/* Unmerged change from project 'Penguin.Threading.BackgroundWorker.Local (net5.0)'
Before:
namespace Penguin.Threading.BackgroundWorker
After:
namespace Penguin.Threading.BackgroundWorker.BackgroundWorker.BackgroundWorker
*/

/* Unmerged change from project 'Penguin.Threading.BackgroundWorker.Local (netstandard2.1)'
Before:
namespace Penguin.Threading.BackgroundWorker
After:
namespace Penguin.Threading.BackgroundWorker.BackgroundWorker
*/
namespace Penguin.Threading.BackgroundWorker
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
                _ = this.ResultTaskSource.TrySetResult(false);
            }
            else
            {
                this.InternalWorker.RunWorkerAsync(argument);
            }

            return this.ResultTaskSource.Task;
        }

        private void InternalWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.DoWork.Invoke(this, (TArgument)e.Argument);
        }

        private void InternalWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _ = this.ResultTaskSource.TrySetResult(true);
        }
    }
}