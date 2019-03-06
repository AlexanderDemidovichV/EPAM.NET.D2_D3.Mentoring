using System;
using System.Threading;

namespace Task1
{
    class Program
    {
        private static readonly string _terminateInput = "exit";

        private static MathExecutor _currentExecutor;

        static void Main()
        {
            string input = string.Empty;
            Console.WriteLine("Enter the upper bound to calculate the sum from 0 to it (type exit to quit):");
            while (!string.Equals(input, _terminateInput,
                StringComparison.InvariantCultureIgnoreCase))
            {
                input = Console.ReadLine();
                if (!string.Equals(input, _terminateInput,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    var parseResult = Int32.TryParse(input, out var upperBound);
                    if (parseResult)
                    {
                        if (_currentExecutor == null
                            || _currentExecutor.IsExecutionCompleted)
                        {
                            _currentExecutor = new MathExecutor(upperBound,
                                new CancellationTokenSource());
                            _currentExecutor.Start();
                        }
                        else
                        {
                            _currentExecutor =
                                _currentExecutor.ReQueue(new MathExecutor(upperBound,
                                    new CancellationTokenSource()));
                        }
                    }
                    else
                    {
                        Console.WriteLine("Unexpected input!");
                    }
                }
            }
        }
    }
}