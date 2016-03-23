using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Manager.Data
{
    // background work only. NO UI
    class ThreadWorkBase
    {
        protected Thread internalThread;
        private bool done;
        private bool started = false;
        private int threadInterval = 1000; // in ms

        public delegate bool ThreadExecuteMethod();

        public ThreadExecuteMethod OnExecute = null;

        public ThreadWorkBase(int threadInterval)
        {
            this.threadInterval = threadInterval;
            internalThread = new Thread(new ThreadStart(this.Execute));
            done = true;
        }

        public void Start()
        {
            if (!started)
            {
                done = false;
                started = true;
                try
                {
                    this.internalThread.Start();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Thread cannot start" + e.Message);
                    throw;
                }
            }
        }

        public void Stop()
        {
            done = true;
            started = false;
            internalThread.Interrupt();
            internalThread.Join();
        }

        public void Execute()
        {
            int ctr = 0;
            while (!done)
            {
                try
                {
                    if (OnExecute != null)
                        OnExecute();
                    Thread.Sleep(threadInterval);
                }
                catch (ThreadInterruptedException)
                {
                    // Thread is asked to stop.
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Thread error" + e.Message);
                    throw;
                }
            }
        }

    }
}
