using System;
using SimulatorClient;
using SimulatorClient.Enums;
using SimulatorClient.EventArgs;
using UnityEngine;

public class WeightSensor : MonoBehaviour, IStopInput
{
 
    [SerializeField] public VolumeSensorEnum sensorName;
    
    public event EventHandler<OnLoadCellValueChangedEventArgs> OnSensorValueChanged;

    private double _weight;
    void Start()
    {
        IsAbortTriggered = false;
        InitialWeight = 0;
    }
    
    public bool Activated { get; set; }
    public bool IsAbortTriggered { get; private set; }
    public double StopThreshold { get; set; }

    public double InitialWeight { get; private set; }

    public double Weight { get; set; }

    public double Volume => Weight - InitialWeight;
 
    // not using On collision enter because it;s triggered before it really touch
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("LvAdapter")) return;

        SetInitialWeight(other.gameObject);

        if (IsAbortTriggered) return;

        IsAbortTriggered = true;

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("LvAdapter")) return;

        InitialWeight = 0;
        Weight = 0;
        OnSensorValueChanged?.Invoke(this, new OnLoadCellValueChangedEventArgs()
        {
            Id = (short)sensorName,
            Weight = Weight
        });
        
        if (IsAbortTriggered == false) return;
        IsAbortTriggered = false;
    }


    public void Clear()
    {
        IsAbortTriggered = false;
    }

  
    public void SetInitialWeight(GameObject tubeAdapter)
    {
       var rgb =  tubeAdapter.GetComponentInChildren<Rigidbody>();
       if (rgb == null) return;
       InitialWeight = rgb.mass;
       Weight = InitialWeight;
       OnSensorValueChanged?.Invoke(this, new OnLoadCellValueChangedEventArgs()
       {
           Id = (short)sensorName,
           Weight = InitialWeight
       });
    }
    
}
