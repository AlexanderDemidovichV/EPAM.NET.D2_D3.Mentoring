using System;

namespace MonitoringServer.Models
{
    public class HandlerModel
    {
        public Guid Guid { get; set; }

        public int Status { get; set; }

        public int Delay { get; set; }
    }
}
