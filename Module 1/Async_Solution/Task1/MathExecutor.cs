using System;
using System.Threading;
using System.Threading.Tasks;

namespace Task1
{
    public class MathExecutor
    {
        private readonly int _upperBound;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private Task _taskInternal;

        public MathExecutor(int upperBound, 
            CancellationTokenSource cancellationTokenSource)
        {
            _upperBound = upperBound;
            _cancellationTokenSource = cancellationTokenSource;
        }

        public void Start()
        {
            _taskInternal = Task.Factory.StartNew(DefaultFunction, 
                _cancellationTokenSource.Token);
        }

        public MathExecutor ReQueue(MathExecutor executor)
        {
            executor._taskInternal = _taskInternal.ContinueWith(_ =>
            {
                Console.WriteLine("Re-calculating...");
                executor.DefaultFunction();
            }, executor._cancellationTokenSource.Token);
            _cancellationTokenSource.Cancel();
            return executor;
        }

        public bool IsExecutionCompleted => _taskInternal?.IsCompleted ?? false;

        private void DefaultFunction()
        {
            int result = 0;
            for (int i = 0; i <= _upperBound; i++)
            {
                result += i;
                Thread.Sleep(50);
                if (_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    return;
                }
            }
            Console.WriteLine($"The sum equals to: {result}");
        }
    }
}
