using System;

namespace SimulatorClient.Protocol
{
    [Serializable]
    public class CleaningStationDto
    {
        public ushort id;
        public string leftTubeAdapter;
        public string cleaningSolution;
        public double totalCleaningVolume;
        public string rightTubeAdapter;
        public string rinseSolution;
        public double totalRinseVolume;

    }
}