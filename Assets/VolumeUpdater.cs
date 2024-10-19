using System;
using JetBrains.Annotations;
using SimulatorClient.Enums;
using SimulatorClient.EventArgs;
using UnityEngine;

public class VolumeUpdater : MonoBehaviour
{
    public event EventHandler<TubeSensorEventArgs> OnSensorStateChanged;

    // Start is called before the first frame update
    [CanBeNull] private Collider _placedObject;
    [SerializeField] public VolumeSensorEnum sensorName;

    [SerializeField] public double volumeOffset;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("DropObject"))
            return;
        var grippedObject = other.GetComponent<GrippedObject>();
        if (grippedObject == null)
            return;
        _placedObject = other;
        OnSensorStateChanged?.Invoke(this, new TubeSensorEventArgs()
        {
            Id = (short)sensorName,
            IsTubeAttached = true,
            Volume = Volume

        });
    }

    public  double Volume
    {
        get
        {
            var tube = GetComponentInChildren<Tube>();
            if (tube != null)
                return tube.Volume;
            
            if (_placedObject == null) return 0; 
            tube = _placedObject.GetComponent<Tube>();
            if (tube == null)
                return 0 + volumeOffset;
            return tube.Volume + volumeOffset;

        }

        set
        {
            var tube = GetComponentInChildren<Tube>();
            if (tube != null)
            {
                tube.Fill((float)value);
                return;
            }

            if (_placedObject == null) return;
            tube = _placedObject.GetComponent<Tube>();
            if (tube == null)
                return;
            var volume = value - volumeOffset;
            tube.Fill((float)volume);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _placedObject = null;
        OnSensorStateChanged?.Invoke(this, new TubeSensorEventArgs()
        {
            Id = (short)sensorName,
            IsTubeAttached = false,
            Volume = 0
        });
    }

    private void OnTriggerStay(Collider other)
    {
       
        var grippedObject =  other.GetComponent<GrippedObject>();

        if (grippedObject == null)
            return;
        
        if (!grippedObject.IsHandedOver) return;
        (grippedObject.transform).SetParent(transform);
        grippedObject.IsHandedOver = false;
    }
}