using JetBrains.Annotations;
using SimulatorClient;
using UnityEngine;
using UnityEngine.Serialization;

public class XAxisMotor : MotorBase<XAxis>
{
  [FormerlySerializedAs("HSpeed")]
  public float hSpeed ; //mm /secs
  public static XAxisMotor Instance { get; private set; }


  
  // Start is called before the first frame update
  [UsedImplicitly]
  void Start()
  {
    MaxTravelLimit = Simulator.MaxTravelLimit.x;
    MinTravelLimit = Simulator.MinTravelLimit.x;
    hSpeed = Simulator.ManualControlSpeed.x;
    StartUp();
    Instance = this;
    RegisterKeyForMoveToMaxTravel(KeyCode.RightArrow, hSpeed);
    RegisterKeyForMoveToMinTravel(KeyCode.LeftArrow, hSpeed);
  }
}