using System;

namespace SimulatorClient.DTOs
{
    [Serializable]
    public class VolumeSensorStateDto:StationBaseDto
    {
        public short id;
        public bool isTubeAttached;
        public double volume;
    }
}