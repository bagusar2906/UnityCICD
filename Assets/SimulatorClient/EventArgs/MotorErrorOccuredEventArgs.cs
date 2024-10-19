namespace SimulatorClient.EventArgs
{
  public class MotorErrorOccuredEventArgs : System.EventArgs
  {
    public short MotorID { get; set; }
    public ushort MotorErrorCode { get; set; }
    public double Position { get; set; }
    public short BustId { get; set; }

    public MotorErrorOccuredEventArgs() { }
    public MotorErrorOccuredEventArgs(short motorID, ushort motorErrorCode)
    {
      MotorID = motorID;
      MotorErrorCode = motorErrorCode;
    }
  }
}