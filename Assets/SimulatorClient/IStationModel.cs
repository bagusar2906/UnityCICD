using SimulatorClient.DTOs;
using SimulatorClient.Enums;

namespace SimulatorClient
{
    public interface IStationModel
    {
        short BusId { get; set; }
        short Id { get; set; }
        IMotorSim Motor0 { get; }
        IMotorSim Motor1 { get; }
       
        ISliderModel SliderModel { get; }
        
        IChipClampModel ChipClampModel { get; }
        
        VolumeUpdater[] VolumeSensors { get; }
        
       // WeightSensor[] WeightSensors { get; }
    

        void UpdateVolume(VolumeSensorDto sensor);
    }
}