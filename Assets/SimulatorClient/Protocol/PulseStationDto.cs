using System;

namespace SimulatorClient.Protocol
{
    [Serializable]
    public class PulseStationDto
    {

        public ushort id;
        public string leftTubeAdapter;
        public string nanoTubeAdapter;
        public string rightTubeAdapter;
        public string bufferSource; // tube, reservoir
        public string bufferType;
        public string bufferPosition;
        public double totalBufferVolume;
    }
}