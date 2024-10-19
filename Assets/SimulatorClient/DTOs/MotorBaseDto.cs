using System;
using UnityEngine.Serialization;

namespace SimulatorClient.DTOs
{
    [Serializable]
    public class MotorBaseDto : StationBaseDto
    {
       public short motorId;
    }
}