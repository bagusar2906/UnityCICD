using SimulatorClient;
using SimulatorClient.DTOs;
using SimulatorClient.Enums;
using UnityEngine;

public class CleaningStationModel : PulseStationModel
{
    [SerializeField] public GameObject peristalticPump2;
    [SerializeField] public GameObject wastePump;
    public override IMotorSim Motor0 => peristalticPump1.GetComponentInChildren<PeristalticMotor>();
    public override IMotorSim Motor1 => peristalticPump2.GetComponentInChildren<PeristalticMotor>();
    public IMotorSim Motor3 =>  wastePump.GetComponentInChildren<WastePumpMotor>();

    public override void UpdateVolume(VolumeSensorDto dto)
    {
        if (dto.id == (int)VolumeSensorEnum.LowVolume)
        {
            var waste = Motor3 as WastePumpMotor;
            if (waste != null)
                waste.LiquidFlowToWaste();
            return;
        }
        base.UpdateVolume(dto);
    }
}