using System;

namespace SimulatorClient.Protocol
{
    [Serializable]
    public class ChipDto
    {

        public string barcode;
        public ushort mwco;
        public LocationDto chipSlot;

    }
}