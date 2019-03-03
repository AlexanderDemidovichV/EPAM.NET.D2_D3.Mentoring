using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task1
{
    class Program
    {
        async static Task Catcher()
        {
            try { await Thrower(); }
            catch (NullReferenceException e)
            {  // Исключение будет обработано здесь 
                throw;
            } 
        }

        async static Task Thrower()
        {
            await Task.Delay(100); 
            throw new NullReferenceException();
        }

        static async Task Main(string[] args)
        {
            try
            {
                await Catcher();
            }
            catch (NullReferenceException e)
            {
                throw;
            }
            

            //var token = new CancellationTokenSource();
            //Console.WriteLine("first");
            //int n = Convert.ToInt32(Console.ReadLine());

            //var task = RunTask(token.Token, n);



            //Console.WriteLine("second");
            //n = Convert.ToInt32(Console.ReadLine());
            //token.Cancel();
            //if (n == 0)
            //{

            //}
            //else
            //{
            //    task.ContinueWith(async _ =>
            //    {
            //        RunTask(new CancellationTokenSource().Token, n);
            //    });
            //    Console.WriteLine("cancel");
            //    token.Cancel();
            //    token = new CancellationTokenSource();
            //    RunTask(token.Token, n);
            //}


            Console.ReadKey();
        }

        private static Task RunTask(CancellationToken token, int number)
        {
            var task = new Task(() =>
            {
                int result = 0;
                for (int i = 0; i <= number; i++)
                {
                    result += i;
                    Thread.Sleep(50);
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                }
                Console.WriteLine($"Result: {result}");
            }, token);
            task.Start();
            return task;
        }
    }
}
