namespace SimulatorClient.EventArgs
{
  public class MotorMoveDoneEventArgs : System.EventArgs
  {
    public short MotorID { get; set; }
    public ushort Status { get; set; }
    public double Position { get; set; }
    public short BusId { get; set; }

    public MotorMoveDoneEventArgs()
    { }

    public MotorMoveDoneEventArgs( short motorId, ushort status, double position )
    {
      MotorID = motorId;
      Status = status;
      Position = position;
    }

  }
}