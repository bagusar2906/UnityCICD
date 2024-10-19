using System;

namespace SimulatorClient.DTOs
{
    [Serializable]
    public abstract class StationBaseDto
    {
        public short busId;
        public short stationId;
    }
}