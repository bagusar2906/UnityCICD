using SimulatorClient.Enums;

namespace SimulatorClient.EventArgs
{
    public class OnSliderStateChangedEventArgs : System.EventArgs
    {
        public SliderState State { get; set; }
        
    }
}