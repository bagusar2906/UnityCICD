using System;
using System.Collections.Generic;

namespace SimulatorClient.Protocol
{
    [Serializable]
    public class ProtocolDto
    {
        public string name;
        public List<StationDto> decks;
        public List<PulseStationDto> pulseStations;
        public List<CleaningStationDto> cleaningStations;
        public List<TubeDto> tubes;
        public List<ChipDto> chips;
    }
}