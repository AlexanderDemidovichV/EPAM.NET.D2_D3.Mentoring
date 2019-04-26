namespace MonitoringServer.Models
{
    public class HandlerViewModel: Entity
    {
        public HandlerStatus Status;

        public HandlerViewModel()
        {
            Type = EntityType.Handler;
        }
    }

    public enum HandlerStatus
    {
        Running,
        Idle,
        FuckedUp
    }
}
