using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Server
{
    public class Server
    {      
        private static TcpListener _tcpListener;
        private readonly ConcurrentDictionary<string, Client> _clients;
        private static readonly int _port = 8888;

        public Server()
        {
            _clients = new ConcurrentDictionary<string, Client>();
            _tcpListener = new TcpListener(IPAddress.Any, _port);            
        }
        
        public void Listen(CancellationToken token)
        {
            try
            {                
                _tcpListener.Start();
                Console.WriteLine("Waiting for connection...");

                while (true)
                {
                    if (!token.IsCancellationRequested)
                    {
                        var tcpClient = _tcpListener.AcceptTcpClient();

                        var client = new Client(tcpClient, this, new CancellationTokenSource());
                        AddConnection(client);

                        var clientTask = new Task(client.Receive);
                        clientTask.Start();
                    }
                    else
                    {
                        token.ThrowIfCancellationRequested();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect(null);
            }
        }
        
        public void Disconnect(CancellationTokenSource tokenSource)
        {
            tokenSource?.Cancel();

            _tcpListener.Stop();

            foreach (var client in _clients)
            {
                client.Value.Close();
            }       
        }

        internal void AddConnection(Client client)
        {
            _clients[client.Id] = client;
        }

        internal void RemoveConnection(string id)
        {
            _clients.TryRemove(id, out _);
        }
        
        internal void BroadcastMessage(string message, string id)
        {            
            foreach (var client in _clients)
            {
                if (client.Key != id)
                {
                    client.Value.SendMessage(message);
                }
            }
        }
    }
}