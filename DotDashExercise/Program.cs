using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotDashExercise
{
    class Program
        // program that simulates a load screen using ManualResetEvent - set and reset by user action.
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();  // CancellationTokenSource object init.
            ManualResetEvent manualResetEventDot = new ManualResetEvent(false);  // ManualResetEvent for Dot class.
            ManualResetEvent manualResetEventDash = new ManualResetEvent(false);  // ManualResetEvent for Dash class.
            Tuple<CancellationToken, ManualResetEvent> objTuple =
                new Tuple<CancellationToken, ManualResetEvent>(cancellationTokenSource.Token, manualResetEventDash);  
            Thread thDot = new Thread(new Dot(cancellationTokenSource.Token, manualResetEventDot).ShowDot);
            thDot.Start();  // run Dot.ShowDot() in new foreground thread.
            Dash dash = new Dash();
            ThreadPool.QueueUserWorkItem((tupObj) => dash.ShowDash(objTuple));  // run Dash.ShowDash() in thread pool.

            Console.WriteLine("Please enter a key: \n");
            while (thDot.IsAlive)  // switch between dash and dot load screens using user input unit foreground thread is cancelled.
            {
                if (Console.ReadKey().Key == ConsoleKey.A)
                {
                    manualResetEventDot.Set();
                }
                else if (Console.ReadKey().Key == ConsoleKey.S)
                {
                    manualResetEventDot.Reset();
                }
                else if (Console.ReadKey().Key == ConsoleKey.Z)
                {
                    manualResetEventDash.Set();
                }
                else if (Console.ReadKey().Key == ConsoleKey.X)
                {
                    manualResetEventDash.Reset();
                }
                else if (Console.ReadKey().Key == ConsoleKey.Q)
                {
                    cancellationTokenSource.Cancel();
                }
            }
            Console.ReadKey();
        }
    }
}
