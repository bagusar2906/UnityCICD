using System;

namespace SimulatorClient.DTOs
{
    [Serializable]
    public class MoveGripperDoneDto: MotorBaseDto
    {
        public double position;
    }
}