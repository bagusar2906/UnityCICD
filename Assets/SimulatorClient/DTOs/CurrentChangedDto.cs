using System;

namespace SimulatorClient.DTOs
{
    [Serializable]
    public class CurrentChangedDto : MotorBaseDto
    {
        public double holdingCurrent;
    }
}