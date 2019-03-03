using System;
using System.Threading;
using System.Threading.Tasks;

namespace Task7
{
    class Program
    {
        static async Task Main()
        {
            TaskC();



            Console.ReadKey();
        }

        private static void TaskA()
        {
            var task = new Task(() =>
            {
                Console.WriteLine("Init task A.");
            });
            
            task.ContinueWith(x =>
            {
                Console.WriteLine("I don't care about result.");
            }, TaskContinuationOptions.None
                | TaskContinuationOptions.AttachedToParent);

            try
            {
                task.Start();
                task.Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void TaskB()
        {
            var task = new Task(() =>
            {
                Console.WriteLine("Init task B.");
                throw new Exception("Booom!!!");
            });
            
            task.ContinueWith(x =>
            {
                Console.WriteLine("I love faulted task.");
            }, TaskContinuationOptions.OnlyOnFaulted 
                | TaskContinuationOptions.AttachedToParent);

            try
            {
                task.Start();
                task.Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void TaskC()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            var token = new CancellationToken();
            var task = new Task(() =>
            {
                Console.WriteLine("Init task C.");
                Console.WriteLine($"Thread #{Thread.CurrentThread.ManagedThreadId}");
                throw new Exception("Booom!!!");
            });
            

            task.ContinueWith(x =>
            {
                Console.WriteLine("I love Faulted and use the same context...");
                Console.WriteLine($"Thread #{Thread.CurrentThread.ManagedThreadId}");
            }, token, TaskContinuationOptions.OnlyOnFaulted
                | TaskContinuationOptions.AttachedToParent, 
            TaskScheduler.FromCurrentSynchronizationContext());

            try
            {
                task.RunSynchronously();
                task.Wait(token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
