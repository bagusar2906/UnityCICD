namespace SimulatorClient.EventArgs
{
  public class GripperCurrentChangedEventArgs : System.EventArgs
  {
    public short MotorID { get; set; }
    public double HoldingCurrent { get; set; }
  }
  public class OnPositionReachedEventArgs : System.EventArgs
  {
    public short MotorID { get; set; }
    public double Position { get; set; }
    public short BusId { get; set; }

    public OnPositionReachedEventArgs() { }
    public OnPositionReachedEventArgs(short motorID, double position)
    {
      MotorID = motorID;
      Position = position;
    }
  }
}