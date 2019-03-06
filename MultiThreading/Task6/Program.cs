using System;
using System.Collections.Generic;
using System.Threading;

namespace Task6
{
    class Program
    {
        private static readonly Semaphore _semaphore1 = new Semaphore(1, 1);
        private static readonly Semaphore _semaphore2 = new Semaphore(0, 1);

        private static List<int> _items = new List<int>();

        private static void Main()
        {
            _items = new List<int>();
            Thread addingThread = new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    _semaphore1.WaitOne();
                    _items.Add(i);
                    Console.WriteLine($"Adding thread says: {i} just have been added to collection!");
                    _semaphore2.Release();
                }
            });

            Thread notifyingThread = new Thread(() =>
            {
                var innerCount = 0;
                while (innerCount < 10)
                {
                    _semaphore2.WaitOne();
                    Console.WriteLine("Notifying thread says: ");
                    _items.ForEach(Console.WriteLine);
                    _semaphore1.Release();
                    innerCount++;
                }
            });

            addingThread.Start();
            notifyingThread.Start();
            notifyingThread.Join();

            Console.ReadLine();
        }
    }

}
