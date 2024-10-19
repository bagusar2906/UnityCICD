using JetBrains.Annotations;
using SimulatorClient;
using UnityEngine;
using UnityEngine.Serialization;

public class ZAxisMotor : MotorBase<ZAxis>
{
    
    
    [FormerlySerializedAs("VSpeed")] 
    public float vSpeed ;

    public static ZAxisMotor Instance { get; private set; }
   
    // Start is called before the first frame update
    // Start is called before the first frame update
    [UsedImplicitly]
    void Start()
    {
        MaxTravelLimit = Simulator.MaxTravelLimit.z;
        MinTravelLimit = Simulator.MinTravelLimit.z;
        vSpeed = Simulator.ManualControlSpeed.z;
        StartUp();
        Instance = this;
        RegisterKeyForMoveToMaxTravel(KeyCode.PageUp, vSpeed);
        RegisterKeyForMoveToMinTravel(KeyCode.PageDown, vSpeed);
    }
    
}
