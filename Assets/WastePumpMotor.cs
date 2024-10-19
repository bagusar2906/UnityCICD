using System;
using System.Collections.Generic;
using SimulatorClient;
using SimulatorClient.Enums;
using SimulatorClient.EventArgs;
using UnityEngine;
using UnityEngine.Serialization;

public class WastePumpMotor : MonoBehaviour, IMotorSim
{

    [SerializeField]
    private ParticleSystem liquidFlow;
    public float rotationSpeed = 100f;

    public Vector3 rotationAxis = Vector3.up;

    private readonly double _updatePositionIntervalInSec = 0.5;
  
    // Start is called before the first frame update
    [FormerlySerializedAs("MotorId")] public short motorId;
    private float _rotationSpeed = 0;
    private double _liquidFlowTime;
    private double _liquidFlowDuration = 500;
    public short BusId { get; set; }
    public short Id => motorId;

    public double CurrentPos { get; }
    public double CurrentVel { get; }
    public double CurrentAccel { get; }
    public bool IsMoving { get; }
    public MotorState State { get; }
    public event EventHandler<MotorHomeDoneEventArgs> MotorHomeDone;
    public event EventHandler<MotorMoveStartedEventArgs> MotorMoveStarted;
    public event EventHandler<MotorMoveDoneEventArgs> MotorMoveDone;
    public event EventHandler<OnPositionReachedEventArgs> OnPositionChanged;
    public event EventHandler<MotorErrorOccuredEventArgs> MotorErrorOccured;

    public void StopMove()
    {
        _rotationSpeed = 0;
        liquidFlow.Stop();
    }

    private void Start()
    {

        var station = GetComponentInParent<CleaningStationModel>();
        if (station != null)
            BusId = station.BusId;
        
        liquidFlow.Stop();
    }

    public void LiquidFlowToWaste()
    {
        if (liquidFlow.isPlaying)
            return;
        liquidFlow.Play();
        _liquidFlowTime = _liquidFlowDuration;

    }

    public void MoveAbs(double pos, double vel)
    {
        
    }

    public void Home()
    {
    }

    public void SetAbortInputs(IEnumerable<IStopInput> stopInputs)
    {
    }

    public bool MotionAbortEnabled { get; set; }
    public List<IStopInput> AbortInputs { get; }
    public void SetServoMotionPosition(double position, double speed)
    {
        throw new NotImplementedException();
    }

    private double _totalTime;
    public ushort MoveVel(double vel, bool forward)
    {
        _rotationSpeed = rotationSpeed;
        if (!(rotationSpeed > 0)) return 0;
        _totalTime = 0;
        
        return 0;
    }

    public void AbortMotor()
    {
        StopMove();
    }

    public void ClearFault()
    {
    }

    void Update()
    {
        if (_rotationSpeed == 0)
            return;

        if (liquidFlow.isPlaying && _liquidFlowTime > 0)
        {
            _liquidFlowTime -= Time.deltaTime;
        }
        else
        {
            liquidFlow.Stop();
        }
        
        transform.Rotate(rotationAxis, _rotationSpeed * Time.deltaTime);
        _totalTime += Time.deltaTime;
        if (!(_totalTime > _updatePositionIntervalInSec)) return;
        _totalTime = 0;
       
    }
}