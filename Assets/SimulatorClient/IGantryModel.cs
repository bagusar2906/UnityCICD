using System.Collections.Generic;

namespace SimulatorClient
{
    public interface IGantryModel
    {
        short Id { get;  }
        IDictionary<short, IMotorSim> MotorAxis { get; }
     
        IMotorServo MotorGripper { get; }
    }
}