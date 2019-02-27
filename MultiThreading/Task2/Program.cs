using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task2
{
    class Program
    {
        static async Task Main()
        {
            var task = new Task<int[]>(() =>
            {
                Console.WriteLine("First task:");
                const int min = 0;
                const int max = 20;
                var randNum = new Random();

                var array = Enumerable.Repeat(0, 9)
                    .Select(i => randNum.Next(min, max))
                    .ToArray();

                foreach (var i in array)
                {
                    Console.WriteLine(i);
                }

                return array;
            });

            var result = task
                .ContinueWith(x =>
                {
                    Console.WriteLine("Second task:");

                    const int min = 0;
                    const int max = 20;
                    var randNum = new Random();
                    var array = x.Result.Select(i => i * randNum.Next(min, max)).ToArray();

                    foreach (var i in array)
                    {
                        Console.WriteLine(i);
                    }

                    return array;
                })
                    .ContinueWith(x =>
                    {
                        Console.WriteLine("Third task:");

                        var array = x.Result.OrderBy(i => i).ToArray();

                        foreach (var i in array)
                        {
                            Console.WriteLine(i);
                        }

                        return array;
                    })
                        .ContinueWith(x =>
                        {
                            Console.WriteLine("Fourth task:");
                            var average = x.Result.Average();
                            Console.WriteLine($"Average = {average}");
                            return average;
                        });

            task.Start();
            result.Wait();
        }
    }
}
