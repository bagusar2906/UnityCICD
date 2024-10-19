using System;

namespace SimulatorClient.Protocol
{
    [Serializable]
    public class TubeDto
    {
        public string barcode; // barcode on tube or chip
        public LocationDto tubeSlot; //ie: A1, B2
        public double startVolume;
        public double startConcentration;
        public string liquidType;

    }
}