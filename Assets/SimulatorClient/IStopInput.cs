namespace SimulatorClient
{
    public interface IStopInput
    {
        bool Activated { get; set; }
        bool IsAbortTriggered { get; }
        double StopThreshold { get; set; }
        void Clear();
    }
}