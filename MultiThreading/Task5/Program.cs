using System;
using System.Threading;
using System.Threading.Tasks;

namespace Task5
{
    class Program
    {
        public static Semaphore Semaphore { get; set; }

        static void Main()
        {
            Semaphore = new Semaphore(1, 1);

            StartThreadWithState(10);

            Console.ReadLine();
        }

        internal static void StartThreadWithState(int number)
        {
            ThreadPool.QueueUserWorkItem(ThreadProc, number);
        }

        private static void ThreadProc(object state)
        {
            Semaphore.WaitOne();
            var value = (int) state;

            Console.WriteLine($"value = {value--}");
            Console.WriteLine("-------------------");
            if (value > 0)
            {
                StartThreadWithState(value);
            }

            Semaphore.Release();
        }
    }
}
