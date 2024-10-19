using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace SimulatorClient.DTOs
{
    [Serializable]
    public class VolumeSensorDto: StationBaseDto
    {
        public short id;
        public double weight;
        public double flowRate;
    }
}