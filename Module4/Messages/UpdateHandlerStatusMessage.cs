using System;

namespace Messages
{
    public class UpdateHandlerStatusMessage
    {
        public UpdateHandlerType Status;
        public Guid HandlerGuid;
    }

    public enum UpdateHandlerType
    {
        Register,
        Receive,
        Unregister
    }
}
