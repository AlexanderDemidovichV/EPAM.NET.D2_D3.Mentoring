using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Task1.Core
{
    class Program
    {
        private const int taskAmount = 100;
        private const int iterationAmount = 1000;

        private static void Main()
        {
            var tasks = new List<Task>();
            for (int i = 0; i < taskAmount; i++)            
            {
                var taskNumber = i;
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 1; j <= iterationAmount; j++)
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
        }
    }
}
