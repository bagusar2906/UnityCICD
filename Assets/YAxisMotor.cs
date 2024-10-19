using JetBrains.Annotations;
using SimulatorClient;
using UnityEngine;
using UnityEngine.Serialization;

public class YAxisMotor : MotorBase<YAxis>
{
  // Start is called before the first frame update
  [FormerlySerializedAs("HSpeed")] 
  public float hSpeed ; //mm /sec
  
  public static YAxisMotor Instance { get; private set; }
  

  [UsedImplicitly]
  void Start()
  {
    busId = 1;
    MaxTravelLimit = Simulator.MaxTravelLimit.y;
    MinTravelLimit = Simulator.MinTravelLimit.y;
    hSpeed = Simulator.ManualControlSpeed.y;
    StartUp();
    Instance = this;
    RegisterKeyForMoveToMaxTravel(KeyCode.UpArrow, hSpeed);
    RegisterKeyForMoveToMinTravel(KeyCode.DownArrow, hSpeed);
  }
}