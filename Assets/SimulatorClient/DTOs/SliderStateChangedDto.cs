using System;
using UnityEngine;

namespace SimulatorClient.DTOs
{
    [Serializable]
    public class StateChangedDto: StationBaseDto
    {
        public short state;
    }
}