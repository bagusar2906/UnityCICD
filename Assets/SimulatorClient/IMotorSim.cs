using System;
using System.Collections.Generic;
using SimulatorClient.Enums;
using SimulatorClient.EventArgs;

namespace SimulatorClient
{
    public interface IMotorSim
    {
       
        short Id { get;  }
        double CurrentPos { get;  }
        double CurrentVel { get;  }
        double CurrentAccel { get; }
        
        bool IsMoving { get; }
        MotorState State { get; }

        event EventHandler<MotorHomeDoneEventArgs> MotorHomeDone;
        event EventHandler<MotorMoveStartedEventArgs> MotorMoveStarted;
        event EventHandler<MotorMoveDoneEventArgs> MotorMoveDone;
        event EventHandler<OnPositionReachedEventArgs> OnPositionChanged;
        event EventHandler<MotorErrorOccuredEventArgs> MotorErrorOccured; 



        void StopMove();
        void MoveAbs(double pos, double vel);
        void Home();
        void SetAbortInputs(IEnumerable<IStopInput> stopInputs);
        bool MotionAbortEnabled { get; set; }
        List<IStopInput> AbortInputs { get; }
        void SetServoMotionPosition(double position, double speed);
        ushort MoveVel(double vel, bool forward);
        void AbortMotor();
        void ClearFault();
    }
}