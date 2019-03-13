using System;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace Server
{
    public class Client
    {
        private string _userName;
        private readonly TcpClient _client;
        private readonly Server _server;
        private NetworkStream _stream;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public Client(TcpClient tcpClient, Server server, 
            CancellationTokenSource cancellationTokenSource)
        {
            Id = Guid.NewGuid().ToString();
            _client = tcpClient;
            _server = server;
            _cancellationTokenSource = cancellationTokenSource;
        }

        internal string Id { get; }

        public void SendMessage(string message)
        {
            if (_stream != null && _stream.CanWrite)
            {
                var dataLength = BitConverter.GetBytes(message.Length * 2);
                _stream.Write(dataLength, 0, dataLength.Length);

                var data = Encoding.Unicode.GetBytes(message);
                _stream.Write(data, 0, data.Length);
            }
        }

        public void Receive()
        {
            try
            {
                _stream = _client.GetStream();                
                string message = GetMessage();
                _userName = message;

                message = _userName + " join to chat";                
                _server.BroadcastMessage(message, this.Id);
                Console.WriteLine(message);

                try
                {
                    while (true)
                    {
                        message = GetMessage();

                        if (!_cancellationTokenSource.Token.IsCancellationRequested)
                        {
                            message = $"{_userName}: {message}";
                            Console.WriteLine(message);
                            _server.BroadcastMessage(message, this.Id);
                        }
                        else
                        {
                            ApproveDisconnect();
                            Close();

                            message = $"{_userName}: leave chat";
                            Console.WriteLine(message);
                            _server.BroadcastMessage(message, this.Id);

                            _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                        }
                    }
                }                
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {                
                _server.RemoveConnection(this.Id);
                Close();
            }
        }
        
        private void ApproveDisconnect()
        {
            if (_stream != null && _stream.CanWrite)
            {
                var data = BitConverter.GetBytes(0);
                _stream.Write(data, 0, data.Length);
            }
        }

        private string GetMessage()
        {
            var data = new byte[512];
            var builder = new StringBuilder();

            _stream.Read(data, 0, 4);
            var messageLength = BitConverter.ToInt32(data, 0);

            if (messageLength == 0)
            {
                _cancellationTokenSource.Cancel();
                return null;
            }

            do
            {
                var bytes = _stream.Read(data, 0, 
                    messageLength < data.Length ? messageLength : data.Length);

                messageLength -= data.Length;
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (_stream.DataAvailable && messageLength > 0);

            return builder.ToString();
        }
        
        internal void Close()
        {
            _stream?.Close();

            _client?.Close();
        }
    }
}
