using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Task3
{
    class Program
    {
        private static void Main()
        {
            double[,] array1 = { 
                {2, 3, 1},
                {2,-7, 4}
               
            };
            double[,] array2 =
            {
                {3, 4, 5},
                {1, 1, 4},
                {2, 1, 4}
            };
            
            MultiplyMatrix(array1, array2);
        }

        private static double[,] result;

        private static void MultiplyMatrix(double[,] a, double[,] b)
        {
            int rA = a.GetLength(0);
            int cA = a.GetLength(1);

            int rB = b.GetLength(0);
            int cB = b.GetLength(1);
            
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            if (cA == rB)
            {
                result = new double[rA, cB];

                Parallel.For(0, rA, i => {
                    Parallel.For(0, cB, j => {
                        result[i, j] = 0;
                        for (int k = 0; k < cA; k++)
                        {
                            result[i, j] += a[i, k] * b[k, j];
                        }
                    });
                });
            }
            else
            {
                Console.WriteLine("Number of columns in the first Matrix should be equal to number of rows in the second one.");
            }
          
            stopwatch.Stop();

            PrintMatrix(result, rA, cB);
            Console.WriteLine($"\nElapsed time - {stopwatch.Elapsed}");
            Console.ReadLine();
        }

        private static void PrintMatrix(double[,] matrix, int matrixRowsAmount, int matrixColumnsAmount)
        {
            for (int i = 0; i < matrixRowsAmount; i++)
            {
                for (int j = 0; j < matrixColumnsAmount; j++)
                {
                    Console.Write($"{matrix[i, j]} ");
                }
                Console.WriteLine();
            }
        }
    }
}
