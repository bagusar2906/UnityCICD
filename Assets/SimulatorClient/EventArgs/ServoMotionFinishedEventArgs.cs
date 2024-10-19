using UnityEngine.UIElements;

namespace SimulatorClient.EventArgs
{
    public class ServoMotionFinishedEventArgs : System.EventArgs
    {
        public ushort Id { get; set; }
        public double Position { get; set; }

        public ServoMotionFinishedEventArgs(short busId, ushort id, double position )
        {
            BusId = busId;
            Id = id;
            Position = position;
        }

        public short BusId { get;  }
    }
}