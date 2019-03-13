using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Task1
{
    class Program
    {
        private const int TaskAmount = 100;
        private const int IterationAmount = 1000;

        private static void Main()
        {
            var tasks = new List<Task>();
            for (int i = 0; i < TaskAmount; i++)
            {
                var taskNumber = i;
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 1; j <= IterationAmount; j++)
                    {
                        Console.WriteLine($"Task #{taskNumber} - iteration #{j}");
                    }
                }));
            }

            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException e)
            {
                foreach (var innerException in e.Flatten().InnerExceptions)
                {
                    Console.WriteLine(innerException.ToString());
                }
            }

            Console.ReadLine();
        }
    }
}
