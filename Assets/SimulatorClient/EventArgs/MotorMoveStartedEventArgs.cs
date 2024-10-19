namespace SimulatorClient.EventArgs
{
  public class MotorMoveStartedEventArgs : System.EventArgs
  {
    public MotorMoveStartedEventArgs() { }
    public MotorMoveStartedEventArgs( short motorID )
    {
      MotorID = motorID;
    }
    public short MotorID { get; set; }
    public short BusId { get; set; }
  }
}