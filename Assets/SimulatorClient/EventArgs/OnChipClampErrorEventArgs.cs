namespace SimulatorClient.EventArgs
{
    public class OnChipClampErrorEventArgs
    {
        public short Operation { get; set; }
        public short State { get; set; }
        public short Error { get; set; }
    }
}