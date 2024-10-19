using System;

namespace SimulatorClient.DTOs
{
    [Serializable]
    public class SetMotionAbortDto: StationBaseDto
    {
        public uint enableMask;
    }
}