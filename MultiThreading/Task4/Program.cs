using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace Task4
{
    public class Program
    {
        private static void Main()
        {
            StartThreadWithState(42, 10, 1);
        }

        internal static void StartThreadWithState(
            int number, int depth, int minDepth)
        {
            ThreadWithState tws = new ThreadWithState(number, depth, minDepth);

            Thread t = new Thread(tws.ThreadProc);
            t.Start();
            t.Join();

            Console.ReadLine();
        }
        
    }

    public class ThreadWithState
    {
        private int _value;
        private int _depth;
        private readonly int _minDepth;

        public ThreadWithState(int number, int depth, int minDepth)
        {
            _value = number;
            _depth = depth;
            _minDepth = minDepth;
        }
        
        public void ThreadProc()
        {
            if (_depth == _minDepth)
            {
                PrintState(--_value, _depth);
                return;
            }
            PrintState(--_value, _depth--);

            Program.StartThreadWithState(_value, _depth, _minDepth);
        }

        private void PrintState(int number, int depth)
        {
            Console.WriteLine($"depth = {depth}");
            Console.WriteLine($"value = {number}");
            Console.WriteLine($"ThreadID = {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("-------------------");
        }
    }
}
