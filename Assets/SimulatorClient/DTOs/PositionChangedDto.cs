using System;

namespace SimulatorClient.DTOs
{
    [Serializable]
    public class PositionChangedDto: MotorBaseDto
    {
        public double position;
    }
}