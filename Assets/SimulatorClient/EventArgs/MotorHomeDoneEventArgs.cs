namespace SimulatorClient.EventArgs
{
  public class MotorHomeDoneEventArgs : System.EventArgs
  {
    public short BusId { get; set; }
    public short MotorID { get; set; }
    public double Position { get; set; }
  }
}