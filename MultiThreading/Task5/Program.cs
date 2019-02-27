using System;
using System.Threading;
using System.Threading.Tasks;

namespace Task5
{
    class Program
    {
        public static Semaphore Pool { get; set; }

        static void Main()
        {
            
            Pool = new Semaphore(10, 10);

            StartThreadWithState(42);
        }

        internal static void StartThreadWithState(int number)
        {
            ThreadPool.QueueUserWorkItem(ThreadProc, 
                 number);
        }

        private static void ThreadProc(object state)
        {
            var value = (int)state;
           
            Console.WriteLine($"value = {--value}");
            Console.WriteLine("-------------------");
            
            Pool.WaitOne();
            StartThreadWithState(value);
        }
    }
    

}
