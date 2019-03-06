using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Task7
{
    class Program
    {
        private static void Main()
        {
            RunPart("A", TaskContinuationOptions.None);
            RunPart("B", TaskContinuationOptions.OnlyOnFaulted);
            RunPart("C", TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
            RunPart("D", TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.RunContinuationsAsynchronously);

            Console.ReadKey();
        }


        private static void RunPart(string partIdentifier, TaskContinuationOptions opts, TaskScheduler scheduler = null)
        {
            Console.WriteLine($"Part {partIdentifier}.");
            Console.WriteLine("No exceptions: ");
            var initial = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Task 7.{partIdentifier} (initial, normal flow, thread #{Thread.CurrentThread.ManagedThreadId})");
            });
            var continuation = initial.ContinueWith(task => Console.WriteLine($"Task 7.{partIdentifier} (continuation, thread #{Thread.CurrentThread.ManagedThreadId})"), opts);
            initial.Wait();
            if (!continuation.IsCanceled)
            {
                continuation.Wait();
            }
            Console.WriteLine("Exception in initial task: ");
            initial = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Task 7.{partIdentifier} (initial, exc thrown, thread #{Thread.CurrentThread.ManagedThreadId})");
                throw new NullReferenceException();
            });
            if (scheduler != null)
            {
                initial.ContinueWith(
                    task => Console.WriteLine(
                        $"Task 7.{partIdentifier} (continuation, thread #{Thread.CurrentThread.ManagedThreadId})"),
                    CancellationToken.None, opts, scheduler).Wait();
            }
            else
            {
                initial.ContinueWith(
                    task => Console.WriteLine(
                        $"Task 7.{partIdentifier} (continuation, thread #{Thread.CurrentThread.ManagedThreadId})"),
                    opts).Wait();
            }
        }
    }

    public class NewThreadTaskScheduler : TaskScheduler, IDisposable
    {
        private readonly BlockingCollection<Task> _tasksCollection = new BlockingCollection<Task>();

        public void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
            _tasksCollection.CompleteAdding();
            _tasksCollection.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override void QueueTask(Task task)
        {
            _tasksCollection.Add(task);
            RunTasks();
        }

        private void RunTasks()
        {
            while (_tasksCollection.Count > 0)
            {
                new Thread(() =>
                {
                    TryExecuteTask(_tasksCollection.Take());
                }).Start();
            }
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return TryExecuteTask(task);
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return _tasksCollection.ToArray();
        }
    }
}
