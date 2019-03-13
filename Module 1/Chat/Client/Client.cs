using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Client
{
    public class Client
    {         
        private string _host;
        private readonly int _port;
        private TcpClient _client;
        private NetworkStream _stream;
        private Task _receiveTask;

        public Client(string name, string host, int port)
        {
            Name = name;
            _host = host;
            _port = port;

            _client = new TcpClient();
        }

        public string Name { get; set; }

        public void Connect()
        {
            try
            {
                _client.Connect(_host, _port);
                _stream = _client.GetStream();

                SendMessage(Name);
                
                _receiveTask = new Task(ReceiveMessage);
                _receiveTask.Start();                
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }
        
        public void SendMessage(string message)
        {
            if (_stream != null && _stream.CanWrite)
            {
                byte[] dataLength = BitConverter.GetBytes(message.Length * 2);
                _stream.Write(dataLength, 0, dataLength.Length);

                byte[] data = Encoding.Unicode.GetBytes(message);
                _stream.Write(data, 0, data.Length);

                Console.WriteLine(string.Format("Me: {0}", message));
            }
        }       

        public void Disconnect()
        {
            if (_stream != null && _stream.CanWrite)
            {
                byte[] data = BitConverter.GetBytes(0);
                _stream.Write(data, 0, data.Length);

                _receiveTask.Wait();
            }
        }

        private void StopReceiving()
        {
            if (_stream != null)
            {
                _stream.Close();
            }

            if (_client != null)
            {
                _client.Close();
            }
        }

        private void ReceiveMessage()
        {
            try
            {
                byte[] data = new byte[512];
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                               
                while (true)
                {
                    _stream.Read(data, 0, 4);
                    int messageLength = BitConverter.ToInt32(data, 0);

                    // must stop receiving 
                    if (messageLength == 0)
                    {
                        break;
                    }

                    do
                    {
                        if (messageLength < data.Length)
                        {
                            bytes = _stream.Read(data, 0, messageLength);
                        }
                        else
                        {
                            bytes = _stream.Read(data, 0, data.Length);
                        }

                        messageLength -= data.Length;
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (_stream.DataAvailable && messageLength > 0);

                    Console.WriteLine(builder.ToString());
                    builder.Clear();
                }                
            }            
            catch (Exception exception)
            {
                Console.WriteLine("Connection lost!");                        
            }
            finally
            {
                StopReceiving();
            }
        }              
    }
}
