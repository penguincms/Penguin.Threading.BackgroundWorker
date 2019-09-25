﻿using System;

namespace Penguin.Threading
{
    public abstract class AbstractBackgroundWorker
    {
        public bool CancellationPending { get; protected set; }
        public bool IsBusy => InternalWorker.IsBusy;

        protected System.ComponentModel.BackgroundWorker InternalWorker { get; set; }

        public AbstractBackgroundWorker()
        {
            InternalWorker = new System.ComponentModel.BackgroundWorker();
        }

        public void CancelAsync()
        {
            CancellationPending = true;
        }
    }
}