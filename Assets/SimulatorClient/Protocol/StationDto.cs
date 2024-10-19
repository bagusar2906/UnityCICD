using System;

namespace SimulatorClient.Protocol
{
    [Serializable]
    public class StationDto
    {
        public int deckId;
        public int rackId;
        public string rackName;
        public string rackType;
        public double rows;
        public double columns;
        public string type ; // "50mL" | "15mL" | "1.5mL" | "Chip"
    }
}