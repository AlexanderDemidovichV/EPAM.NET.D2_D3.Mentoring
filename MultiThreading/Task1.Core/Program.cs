using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Task1.Core
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Hello World!");

            var tasks = DoWork();
            
            //foreach (Task task in tasks)
            //{
            //    task.Start();
            //}
            Parallel.ForEach(tasks, (t) => { t.Start(); });
            Task.WaitAll(tasks);
        }

        public static Task[] DoWork()
        {
            int[] array = Enumerable.Range(0, 100).ToArray();
            Task[] tasks = new Task[100];

            array.ToList().ForEach((i) =>
            {
                tasks[i] = new Task(() =>
                {
                    DoSomething(i);
                });
            });
            return tasks;
        }

        static void DoSomething(int taskNumber)
        {
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine($"Task #{taskNumber} – {i}");
            }
        }
    }
}
