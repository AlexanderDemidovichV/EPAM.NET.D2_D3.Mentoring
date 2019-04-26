using System;

namespace Messages
{
    public class UpdateHandlerStatusMessage
    {
        public UpdateHandlerType Status;
        public Guid HandlerGuid;
    }
}
