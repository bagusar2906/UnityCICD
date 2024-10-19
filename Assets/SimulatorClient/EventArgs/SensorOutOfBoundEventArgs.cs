namespace SimulatorClient.EventArgs
{
    public class SensorOutOfBoundEventArgs : System.EventArgs
    {
        public ushort Id { get; set; }
        public ushort LimitType { get; set; }
        public double Value { get; set; }
        public SensorOutOfBoundEventArgs()
        { }
        public SensorOutOfBoundEventArgs( ushort id, ushort limitType, double value )
        {
            Id = id;
            LimitType = limitType;
            Value = value;
        }
    }
}