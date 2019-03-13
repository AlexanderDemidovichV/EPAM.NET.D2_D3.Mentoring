using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleServer
{
    class Program
    {
        private static Server.Server _server;
        private static Task _listenTask;
        private static CancellationTokenSource _cancellationTokenSource;
        static void Main(string[] args)
        {
            try
            {                
                _server = new Server.Server();
                _cancellationTokenSource = new CancellationTokenSource();
                _listenTask = new Task(() => 
                    _server.Listen(_cancellationTokenSource.Token));
                _listenTask.Start();
                _listenTask.Wait();
            }
            catch (Exception ex)
            {                
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _server.Disconnect(_cancellationTokenSource);
            }
        }
    }
}
