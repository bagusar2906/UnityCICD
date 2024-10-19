using System;
using JetBrains.Annotations;
using SimulatorClient;
using SimulatorClient.Enums;
using SimulatorClient.EventArgs;
using UnityEngine;
using UnityEngine.Serialization;

public class GripperMotor : MonoBehaviour, IMotorServo
{

    [SerializeField] public short busId;
    [FormerlySerializedAs("MotorId")] public short motorId;


    public event EventHandler<MotorErrorOccuredEventArgs> MotorErrorOccured;
    public event EventHandler<GripperCurrentChangedEventArgs> OnnHoldingCurrentChanged;
    public short Id => motorId;
    public double InitialPos { get; set; }

    public double CurrentPos
    {
        get => _gripperJaw.CurrentPos;
        set => _gripperJaw.CurrentPos = value;
    }

    public bool MotionAbortEnabled
    {
        get => _gripperJaw.MotionAbortEnabled;
        set => _gripperJaw.MotionAbortEnabled = value;
    }


    public double CurrentVel { get; }

    public double CurrentAccel { get; }

    private MotorState _state;
    public MotorState State => _gripperJaw.State; 
    
    public event EventHandler<MotorMoveStartedEventArgs> MotorMoveStarted;
    public event EventHandler<MotorMoveDoneEventArgs> MotorMoveDone;
    public event EventHandler<ServoMotionFinishedEventArgs> OnServoMotionFinished;

  
    private MotorState _prevState = MotorState.Idle;
    private GripperJaw _gripperJaw;

    [UsedImplicitly]
    void Start()
    {

        BusId = 4;
        _gripperJaw = GetComponentInChildren<GripperJaw>();

        _gripperJaw.OnHoldingCurrentChanged += (sender, args) =>
        {
           OnnHoldingCurrentChanged?.Invoke(this, new GripperCurrentChangedEventArgs()
           {
               MotorID = args.MotorID,
               HoldingCurrent = args.HoldingCurrent
           });
        };

    _gripperJaw.MotorMoveDone += (sender, args) =>
        {
            OnServoMotionFinished?.Invoke(this, new ServoMotionFinishedEventArgs(BusId, (ushort)args.MotorID, args.Position));
        };

        _gripperJaw.MotorErrorOccured += (sender, args) =>
        {
            MotorErrorOccured?.Invoke(this, new MotorErrorOccuredEventArgs()
            {
                BustId = BusId,
                MotorID = args.MotorID,
                Position = args.Position,
                MotorErrorCode = args.MotorErrorCode
            });
        };
    }

    public short BusId { get; set; }


    // Update is called once per frame
    [UsedImplicitly]
    void Update()
    {
        
        if (_prevState == State) return;
        _prevState = State;
        
        if (State is MotorState.Moving or not MotorState.Stopped) return;

        var gripper = _gripperJaw;
            //gripper.HeldObj.transform.parent = null;
            if (gripper.IsMoving)
                gripper.StopMove();

            //  OnServoMotionFinished?.Invoke(this, new ServoMotionFinishedEventArgs((ushort)Id, CurrentPos));
        
       
    }


    public void Home(double velocity)
    {
        _gripperJaw.Home();
    }


    
    public void SetServoMotionPosition(double position, double speed)
    {
       
        _gripperJaw.MoveAbs(position, 100 * speed);

    }

    public void ClearFault()
    {
        _gripperJaw.ClearFault();
    }
}
