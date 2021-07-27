using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotDashExercise
{
    public class Dash
    {
        public void ShowDash(Tuple<CancellationToken, ManualResetEvent> obj)  // method to simulate "dash" load screen.
        {
            var cancellationToken = obj.Item1;
            var manualResetEvent = obj.Item2;
            cancellationToken.Register((myThread) => ((Thread)myThread).Interrupt(), Thread.CurrentThread);
            // thread cancelled when CancellationTokenSource.Cancel() is signalled.
            while (true)
            {
                try
                {
                    manualResetEvent.WaitOne();  // waits on the signal from ManualResetEvent before load screen simulation begins.
                    Console.Write("-");  // writes a dot to the console every 3s.
                    Thread.Sleep(3000);
                }
                catch (ThreadInterruptedException)  // catch exception thrown by thread interruption.
                {
                    Console.WriteLine("ShowDash thread interrupted.\n");
                }

                if (cancellationToken.IsCancellationRequested)
                    break;  // thread ends if cancel is signalled.
            }
        }
    }
}
