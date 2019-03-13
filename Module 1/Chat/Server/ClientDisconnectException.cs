using System;

namespace Server
{
    public class ClientDisconnectException : Exception
    {
        public ClientDisconnectException(string message) : base(message)
        {
        }
    }
}
