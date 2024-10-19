using System;
using System.Collections.Generic;
using SimulatorClient.Enums;
using SimulatorClient.EventArgs;
using UnityEngine;
using UnityEngine.Serialization;

public interface ISliderModel
{
    event EventHandler<OnSliderStateChangedEventArgs> OnSliderStateChanged;
    void Move(SliderState sliderState);
}

public class SliderModel : MonoBehaviour, ISliderModel
{
    [FormerlySerializedAs("Forward Position")] public float forwardPosition ;
    [FormerlySerializedAs("Speed")] public float speed = 1f;
    [FormerlySerializedAs("State")] public SliderState newState;

    public ushort Id { get; set; }
    public event EventHandler<OnSliderStateChangedEventArgs> OnSliderStateChanged;
    
    private SliderState _currentState;

    private readonly IDictionary<SliderState, float>
        _positionMap = new Dictionary<SliderState, float>();

    // Start is called before the first frame update
    void Start()
    {
        _currentState = newState;
        _positionMap[SliderState.Backward] = transform.localPosition.z;
        _positionMap[SliderState.Forward] = forwardPosition;
    }

    public void Move(SliderState sliderState)
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
            OnSliderStateChanged?.Invoke(this, new OnSliderStateChangedEventArgs()
                {
                    State = _currentState
                }
            );
    }
    
}