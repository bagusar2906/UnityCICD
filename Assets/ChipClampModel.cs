using System;
using System.Collections.Generic;
using SimulatorClient.Enums;
using SimulatorClient.EventArgs;
using UnityEngine;
using UnityEngine.Serialization;


public interface IChipClampModel
{
    event EventHandler<OnChipClampStateChangedEventArgs> OnChipClampStateChanged;
    void Move(ClampState sliderState);
}

public class ChipClampModel : MonoBehaviour, IChipClampModel
{
    [FormerlySerializedAs("Forward Position")] public float forwardPosition ;
    [FormerlySerializedAs("Speed")] public float speed = 1f;
    [FormerlySerializedAs("State")] public ClampState newState;

   
    public event EventHandler<OnChipClampStateChangedEventArgs> OnChipClampStateChanged;
    
    private ClampState _currentState;

    private readonly IDictionary<ClampState, float>
        _positionMap = new Dictionary<ClampState, float>();

    // Start is called before the first frame update
    void Start()
    {
        _currentState = newState;
        _positionMap[ClampState.UnClamp] = transform.localPosition.z;
        _positionMap[ClampState.Clamp] = forwardPosition;
    }

    public void Move(ClampState sliderState)
    {
        newState = sliderState;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentState == newState)
            return;
        var localPosition = transform.localPosition;
        var targetPosition = new Vector3(localPosition.x, localPosition.y, _positionMap[newState]);
        localPosition = Vector3.MoveTowards(localPosition, targetPosition, speed * Time.deltaTime);
        transform.localPosition = localPosition;

        _currentState = Vector3.Distance(localPosition, targetPosition) < 0.001 ? newState : _currentState;
        if (_currentState == newState)
            OnChipClampStateChanged?.Invoke(this, new OnChipClampStateChangedEventArgs()
                {
                    State = _currentState
                }
            );
    }
    
}
