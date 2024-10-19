using System;
using System.Collections.Generic;

namespace SimulatorClient.DTOs
{
    [Serializable]
    public class RequestInfoDto
    {
        public string message;

        public List<StationConfig> stations;
        public List<string> protocols;
    }
}