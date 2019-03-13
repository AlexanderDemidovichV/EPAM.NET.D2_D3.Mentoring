using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Task6
{
    class Program
    {
       
        private static SynchronizedObservableCollection<int> list;
        static void Main()
        {
            list = new SynchronizedObservableCollection<int>();

            var taskListner = new Task(() =>
            {
                list.CollectionChanged += (sender, e) =>
                {
                    foreach (var item in e.NewItems)
                    {
                        Console.WriteLine($"privet {item}");
                    }

                };

            });

            var task = new Task(() =>
            {
                const int min = 0;
                const int max = 20;
                var randNum = new Random();

                foreach (var item in Enumerable.Repeat(0, 9)
                    .Select(i => randNum.Next(min, max)))
                {
                    list.Add(item);
                }

            });
            taskListner.Start();
            task.Start();
            task.Wait();
        }


    }
    

}
