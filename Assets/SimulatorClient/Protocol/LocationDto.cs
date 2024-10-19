using System;

namespace SimulatorClient.Protocol
{
    [Serializable]
    public class LocationDto
    {

        public string slot;//A1 B2
        public int deckId; //1,2,3
        public int rackId;

    }
}