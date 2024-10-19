namespace SimulatorClient.EventArgs
{
    public class OnLoadCellValueChangedEventArgs : System.EventArgs
    {
        public double Weight { get; set; }
        public short Id { get; set; }
    }
}