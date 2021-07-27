using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotDashExercise
{
    public class Dot
    {
        private readonly CancellationToken cancellationToken;
        private readonly ManualResetEvent manualResetEvent;

        public Dot(CancellationToken _cancellationToken, ManualResetEvent _manualResetEvent) 
        {
            cancellationToken = _cancellationToken;
            manualResetEvent = _manualResetEvent;
        }

        public void ShowDot()  // method to simulate "dot" load screen.
        {
            cancellationToken.Register((myThread) => ((Thread)myThread).Interrupt(), Thread.CurrentThread);
            // thread cancelled when CancellationTokenSource.Cancel() is signalled.
            while (true)
            {
                
                try
                {
                    manualResetEvent.WaitOne();  // waits on the signal from ManualResetEvent before load screen simulation begins.
                    Console.Write(".");  // writes a dash to the console every second.
                    Thread.Sleep(1000);
                }
                catch (ThreadInterruptedException)  // catch exception thrown by thread interruption.
                {
                    Console.WriteLine("ShowDot thread interrupted.\n");
                }

                if (cancellationToken.IsCancellationRequested)
                    break;  // thread ends if cancel is signalled.
            }

        }
    }
}
