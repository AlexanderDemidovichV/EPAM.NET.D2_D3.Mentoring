using System;

namespace MonitoringServer.Models
{
    public abstract class Entity
    {
        public string Guid { get; set; }
        public double Duration { get; set; }
        public EntityType Type { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
