using System;
using SimulatorClient.EventArgs;

namespace SimulatorClient
{
    public interface IMotorServo
    {
        event EventHandler<ServoMotionFinishedEventArgs> OnServoMotionFinished;
        event EventHandler<MotorErrorOccuredEventArgs> MotorErrorOccured;
        
        event EventHandler<GripperCurrentChangedEventArgs> OnnHoldingCurrentChanged;
        
        short Id { get; }

        double InitialPos { get; set; }
        double CurrentPos { get; set; }
        bool MotionAbortEnabled { get; set; }

        void SetServoMotionPosition(double position, double speed);
        void ClearFault();
    }
}