using System;

namespace SimulatorClient.DTOs
{
    [Serializable]
    public class MotorErrorDto : MotorBaseDto
    {
        public ushort errorCode;
        public double position;
    }
}