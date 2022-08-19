namespace DashboardSIMService.Messaging.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}