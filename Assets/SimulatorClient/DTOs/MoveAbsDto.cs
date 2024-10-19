using System;
using UnityEngine.Serialization;

namespace SimulatorClient.DTOs
{
    [Serializable]
    public class MoveAbsDto : MotorBaseDto
    {
        public double position;
        public double velocity;

    }
}