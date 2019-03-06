using System;
using System.Linq;
using System.Threading.Tasks;

namespace Task2
{
    class Program
    {
        private const int MaxRandomValue = 10;
        private const int MinRandomValue = 0;

        private static void Main()
        {
            Task.Run(() => DoTask1())
                .ContinueWith(task => DoTask2(task.Result))
                .ContinueWith(task => DoTask3(task.Result))
                .ContinueWith(task => DoTask4(task.Result)).Wait();

            Console.ReadLine();
        }

        private static int[] DoTask1()
        {
            Console.WriteLine("First task:");
            var randNum = new Random();
            var array = new int[10];

            for (int i = 0; i < 10; i++)
            {
                array[i] = randNum.Next(MaxRandomValue);
            }

            PrintArrayToConsole(array);

            return array;
        }

        private static int[] DoTask2(int[] input)
        {
            Console.WriteLine("Second task:");
            
            var randNum = new Random();
            var array = input
                .Select(i => i * randNum.Next(MinRandomValue, MaxRandomValue))
                .ToArray();

            PrintArrayToConsole(array);

            return array;
        }

        private static int[] DoTask3(int[] array)
        {
            Console.WriteLine("Third task:");
            Array.Sort(array);

            PrintArrayToConsole(array);

            return array;
        }

        private static void DoTask4(int[] input)
        {
            Console.WriteLine("Fourth task:");
            Console.WriteLine($"Average = {input.Average()}");
        }

        private static void PrintArrayToConsole(int[] array)
        {
            foreach (var value in array)
            {
                Console.WriteLine(value);
            }
        }
    }
}
