using SimulatorClient.Enums;

namespace SimulatorClient.EventArgs
{
    public class OnChipClampStateChangedEventArgs : System.EventArgs
    {
        public ClampState State { get; set; }
    }

    public enum TouchSensorState
    {
        CollisionEnter,
        CollisionExit
    }
}