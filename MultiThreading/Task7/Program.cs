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
            RunPart("C", TaskContinuationOptions.OnlyOnFaulted 
                | TaskContinuationOptions.ExecuteSynchronously);
            RunPart("D", TaskContinuationOptions.OnlyOnFaulted, 
                TaskCreationOptions.RunContinuationsAsynchronously);

            Console.ReadKey();
        }
       

        private static void RunPart(string partIdentifier, 
            TaskContinuationOptions opts, 
            TaskCreationOptions tcp = TaskCreationOptions.None)
        {
            Console.WriteLine($"Part {partIdentifier}.");
            Console.WriteLine("No exceptions: ");
            var initial = Task.Factory.StartNew(() =>
            {
                Console.WriteLine(
                    $"Task 7.{partIdentifier} (initial, normal flow, thread #{Thread.CurrentThread.ManagedThreadId})");
            }, tcp);
            var continuation = initial.ContinueWith(task =>
            {
                Console.WriteLine(
                        $"Task 7.{partIdentifier} (continuation, thread #{Thread.CurrentThread.ManagedThreadId})");
            }, opts);
            initial.Wait();
            if (!continuation.IsCanceled)
            {
                continuation.Wait();
            }
            Console.WriteLine("Exception in initial task: ");
            initial = Task.Factory.StartNew(() =>
            {
                Console.WriteLine(
                    $"Task 7.{partIdentifier} (initial, exc thrown, thread #{Thread.CurrentThread.ManagedThreadId})");
                throw new NullReferenceException();
            });
        
            initial.ContinueWith(
                task => Console.WriteLine(
                    $"Task 7.{partIdentifier} (continuation, thread #{Thread.CurrentThread.ManagedThreadId})"),
                opts).Wait();
            
        }
    }
    
}
