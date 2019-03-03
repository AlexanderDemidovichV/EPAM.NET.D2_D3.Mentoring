using System;
using System.Threading;
using System.Threading.Tasks;

namespace Task5
{
    class Program
    {
        public static Semaphore Pool { get; set; }

        public static Semaphore Semaphore { get; set; }

        static void Main()
        {

            Pool = new Semaphore(1, 1);
            Semaphore = new Semaphore(0, 1);

            StartThreadWithState(10);
            Semaphore.WaitOne();
        }

        internal static void StartThreadWithState(int number)
        {
            ThreadPool.QueueUserWorkItem(ThreadProc, number);
        }

        private static void ThreadProc(object state)
        {
            Pool.WaitOne();
            var value = (int) state;

            Console.WriteLine($"value = {value--}");
            Console.WriteLine("-------------------");
            if (value > 0)
            {
                StartThreadWithState(value);
            }
            else
            {
                ThreadPool.QueueUserWorkItem(x => Semaphore.Release());
            }
            Pool.Release();
        }
    }
}
