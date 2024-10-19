using System.Collections.Generic;
using SimulatorClient;
using UnityEngine;

public class GantryModel : MonoBehaviour, IGantryModel
{
    private IDictionary<short, IMotorSim> _motorAxis;

    public short Id => 0;

    public IDictionary<short, IMotorSim> MotorAxis
    {
        get
        {
            if (_motorAxis != null)
                return _motorAxis;
            
            var motorX = GetComponentInChildren<XAxisMotor>();
            var motorY = GetComponentInChildren<YAxisMotor>();
            var motorZ = GetComponentInChildren<ZAxisMotor>();
            //enable motion abort
            motorX.MotionAbortEnabled = true;
            motorY.MotionAbortEnabled = true;
            motorZ.MotionAbortEnabled = true;
            _motorAxis = new Dictionary<short, IMotorSim>()
            {
                { motorX.busId, motorX },
                { motorY.busId, motorY },
                { motorZ.busId, motorZ }
            };
            
            return _motorAxis;
        }
    }


    public IMotorServo MotorGripper => GetComponentInChildren<GripperMotor>();
}
