using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace Client
{
    public class ChatBot : Client
    {
        private readonly Random _random;
        private readonly int _maxMessageCount;
        private readonly string _phraseFilePath;
        private string[] _phraseStorage;

        public ChatBot(
            string name, 
            string host, 
            int port, 
            int maxMessageCount, 
            string phraseFilePath) : base(name, host, port)
        {
            _maxMessageCount = maxMessageCount;
            _phraseFilePath = phraseFilePath;
            _random = new Random((int)DateTime.Now.Ticks);
        }

        public void StartChatRoutine(CancellationToken token)
        {
            try
            {
                LoadPhrases();

                int messageCount = _random.Next(0, _maxMessageCount);

                for (int i = 0; i < messageCount; i++)
                {
                    if (!token.IsCancellationRequested)
                    {
                        SendMessage(_phraseStorage[_random.Next(0, _phraseStorage.Length)]);
                        Thread.Sleep(_random.Next(1000, 5000));
                    }
                    else
                    {
                        token.ThrowIfCancellationRequested();
                    }
                }

                Disconnect();              
            }
            catch (Exception e)
            {
                Console.WriteLine($"{this.Name} fell with an error: {e.Message}");
            }
        }

        private void LoadPhrases()
        {
            if (File.Exists(_phraseFilePath))
            {
                _phraseStorage = File.ReadAllLines(_phraseFilePath);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }
    }
}
