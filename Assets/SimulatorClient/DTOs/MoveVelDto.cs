using System;

namespace SimulatorClient.DTOs
{
    [Serializable]
    public class MoveVelDto : MotorBaseDto
    {
        public double velocity;
        public bool isForward;
    }
}