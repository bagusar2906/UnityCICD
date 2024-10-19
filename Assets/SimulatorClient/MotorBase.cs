using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;
using SimulatorClient.Enums;
using SimulatorClient.EventArgs;
using UnityEngine;
using UnityEngine.Serialization;

namespace SimulatorClient
{
    public abstract class MotorBase<T> : MonoBehaviour, IMotorSim where T : IAxis, new()
    {
        private Vector3 _targetPosition;

        [SerializeField] public GameObject abortInput;
        protected MotorBase()
        {
            AbortInputs = new List<IStopInput>();
        }

        public double TargetPosition { get; private set; }
        protected double InitialPosition { get; private set; }
        private Vector3 _direction;
        private float _speed;
        private SynchronizationContext _syncContext;

        protected bool IsTouching { get; set; }

        public List<IStopInput> AbortInputs { get; private set; }

        public void SetServoMotionPosition(double position, double speed)
        {
            throw new NotImplementedException();
        }

        public ushort MoveVel(double vel, bool forward)
        {
            return 0;
        }

        public void AbortMotor()
        {
        }

        public void ClearFault()
        {
            if (AbortInputs == null) return;
            foreach (var stopInput in AbortInputs)
            {
                stopInput.Clear();
            }
        }

        private bool _isKeyPressed;
        protected T Axis;
        private KeyCode _maxTravelKey;
        private KeyCode _minTravelKey;
        private float _manualControlSpeed;
        private bool _isHoming;


        [SerializeField] public short busId;
        [FormerlySerializedAs("MotorId")] public short motorId;

        public short Id => motorId;
        public double CurrentPos { get; set; }
        public double CurrentVel => _speed;
        public double CurrentAccel { get; }
        public MotorState State { get; private set; }
        public event EventHandler<MotorHomeDoneEventArgs> MotorHomeDone;
        public event EventHandler<MotorMoveStartedEventArgs> MotorMoveStarted;
        public event EventHandler<MotorMoveDoneEventArgs> MotorMoveDone;
        public event EventHandler<OnPositionReachedEventArgs> OnPositionChanged;
        public event EventHandler<MotorErrorOccuredEventArgs> MotorErrorOccured;

        public float MaxTravelLimit { get; set; }
        public float MinTravelLimit { get; set; }

        protected void StartUp()
        {
            _syncContext = SynchronizationContext.Current;

         
            Axis = new T();
            State = MotorState.Idle;
            CurrentPos = Vector3.Dot(ToEngVector(transform.localPosition), Axis.Axis);
            if (abortInput != null)
            {
                AbortInputs.Add(abortInput.GetComponent<CollisionSensor>());
            }
        }

        public bool IsMoving { get; private set; }

        protected Vector3 CurrentPosition
        {
            get
            {
                Vector3 pos = default;
                // On your worker thread
                _syncContext.Send(_ =>
                {
                    var localPosition = transform.localPosition;
                    pos.x = localPosition.z;
                    pos.z = localPosition.y;
                    pos.y = localPosition.x;
                }, null);
                return pos;
            }
        }


        protected virtual Vector3 ToUnity(Vector3 engVector)
        {
            return new Vector3(engVector.y, 200f - engVector.z, engVector.x) / 1000;
        }

        protected virtual Vector3 ToEngVector(Vector3 unity)
        {
            return new Vector3(unity.z, unity.x, 0.2f - unity.y) * 1000;
        }


        private void FixedUpdate()
        {
            if (State == MotorState.Started)
                State = MotorState.Moving;

            var localPosition = transform.localPosition;
            if (IsMoving || _isKeyPressed)
            {
               
                if (MotionAbortEnabled && (AbortInputs.Any(stopInput => stopInput.IsAbortTriggered) || IsTouching))
                {
                    
                    if (_isHoming)
                    {
                        _isHoming = false;
                        MotorHomeDone?.Invoke(this, new MotorHomeDoneEventArgs()
                        {
                            BusId = busId,
                            MotorID = Id,
                            Position = 0
                        });
                        IsMoving = false;
                        State = MotorState.Stopped;
                        return;
                    }

                    CurrentPos = Vector3.Dot(ToEngVector(localPosition), Axis.Axis);
                    MotorErrorOccured?.Invoke(this, new MotorErrorOccuredEventArgs()
                    {
                        BustId = busId,
                        MotorID = Id,
                        Position = CurrentPos,
                        MotorErrorCode = (ushort)MotorErrorEnum.MotionAbort
                    });
                    IsMoving = false;
                    State = MotorState.Stopped;
                    Debug.Log($"Motion Abort is triggered Id: {Id}, pos: {CurrentPos}");
                    return;
                }

                localPosition = Vector3.MoveTowards(localPosition, _targetPosition, _speed * Time.deltaTime);
                transform.localPosition = localPosition;

                if (!_isKeyPressed)
                    IsMoving = Vector3.Distance(localPosition, _targetPosition) > 0.00001;

                CurrentPos = Vector3.Dot(ToEngVector(localPosition), Axis.Axis);

                if (!IsMoving && !_isKeyPressed)
                {
                    if (State == MotorState.Moving)
                        State = MotorState.Stopped;


                    if (_isHoming)
                    {
                        Debug.Log($"[BusId:{busId}]Home Done Id: {Id}");
                        _isHoming = false;
                        MotorHomeDone?.Invoke(this, new MotorHomeDoneEventArgs()
                        {
                            BusId = busId,
                            MotorID = Id,
                            Position = 0
                        });
                        IsMoving = false;
                        State = MotorState.Stopped;
                        return;
                    }

                    Debug.Log($"[BusId:{busId}]Move Done Id: {Id}, pos: {CurrentPos}");
                    MotorMoveDone?.Invoke(this, new MotorMoveDoneEventArgs()
                    {
                        BusId = busId,
                        MotorID = Id,
                        Position = CurrentPos,
                        Status = 0
                    });
                }
            }

            //if moved by command, skip listening to key pressed
            if (IsMoving)
                return;

            if (Input.GetKey(_maxTravelKey))
            {
                _targetPosition = TargetPositionToVector(MaxTravelLimit);
                _speed = _manualControlSpeed;
                _isKeyPressed = true;
                return;
            }

            if (Input.GetKey(_minTravelKey))
            {
                _targetPosition = TargetPositionToVector(MinTravelLimit);
                _speed = _manualControlSpeed;
                _isKeyPressed = true;
                return;
            }

            _isKeyPressed = false;
        }


        public void StopMove()
        {
            State = MotorState.Stopped;
            _speed = 0;
            _targetPosition = transform.localPosition;
            Debug.Log($"Stop Move S Id: {Id}");
        }

        public virtual void MoveAbs(double pos, double vel)
        {
            TargetPosition = pos;
            State = MotorState.Started;
            CurrentPos = Vector3.Dot(ToEngVector(CurrentPosition), Axis.Axis);
            InitialPosition = CurrentPos;
            // On your worker thread
            _syncContext.Post(_ =>
            {
                _isHoming = false;
                Debug.Log($"[BusId:{busId}]Move Started Id: {Id}, curr pos: {CurrentPos}, target pos: {pos}");
                MotorMoveStarted?.Invoke(this, new MotorMoveStartedEventArgs()
                {
                    BusId = busId,
                    MotorID = Id
                });
                IsMoving = true;
                // This code here will run on the main thread
                _targetPosition = TargetPositionToVector(pos);

                _speed = (float)vel / 1000;
            }, null);
        }

        private Vector3 TargetPositionToVector(double pos)
        {
            return ToUnity(Axis.SetAxisValue(ToEngVector(transform.localPosition), (float)pos));
        }

        public virtual void Home()
        {
            State = MotorState.Started;
            _syncContext.Post(_ =>
            {
                Debug.Log($"[BusId:{busId}]Home Id: {Id}");
                _isHoming = true;
                // This code here will run on the main thread
                IsMoving = true;
                _targetPosition = TargetPositionToVector(0);
                _speed = 2f;
            }, null);
        }

        public void SetAbortInputs(IEnumerable<IStopInput> stopInputs)
        {
            AbortInputs.Clear();
            AbortInputs.AddRange(stopInputs);
        }

        protected void RegisterKeyForMoveToMaxTravel(KeyCode keyCode, float manualControlSpeed)
        {
            _maxTravelKey = keyCode;
            _manualControlSpeed = manualControlSpeed / 1000;
        }

        protected void RegisterKeyForMoveToMinTravel(KeyCode keyCode, float manualControlSpeed)
        {
            _minTravelKey = keyCode;
            _manualControlSpeed = manualControlSpeed / 1000;
        }

        public bool MotionAbortEnabled { get; set; }
    }
}