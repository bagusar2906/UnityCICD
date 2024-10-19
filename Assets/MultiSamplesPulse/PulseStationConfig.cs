using System;

namespace MultiSamplesPulse
{
    [Serializable]
    public class PulseStationConfig
    {
      
        // ReSharper disable once InconsistentNaming
        public int id;
        
        // ReSharper disable once InconsistentNaming
        public string leftTubeAdapter;
        // ReSharper disable once InconsistentNaming
        public string nanoTubeAdapter;
        
        // ReSharper disable once InconsistentNaming
        public string rightTubeAdapter;
    }
}