namespace SimulatorClient.EventArgs
{
    public class TubeSensorEventArgs : System.EventArgs
    {
        public short Id { get; set; }
        public double Volume { get; set; }
        public bool IsTubeAttached { get; set; }
    }
}