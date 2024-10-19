using System;

namespace SimulatorClient.DTOs
{
    [Serializable]
    public class LoadCellValueDto:StationBaseDto
    {
        public short id;
        public double weight;
    }
}