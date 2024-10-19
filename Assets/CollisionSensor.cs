using System;
using JetBrains.Annotations;
using SimulatorClient;
using UnityEngine;

public class CollisionSensor : MonoBehaviour, IStopInput
{
    // Start is called before the first frame update
    void Start()
    {
        Activated = true;
    }

    // Update is called once per frame
    
    public bool Activated { get; set; }
    public bool IsAbortTriggered { get; private set; }
    public double StopThreshold { get; set; }
    public void Clear()
    {
        IsAbortTriggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("DragObject"))
            return;
        IsAbortTriggered = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("DragObject"))
            return;
        IsAbortTriggered = false;
    }

    // this will be triggered from external
    [UsedImplicitly]
    public void TriggerCollision()
    {
        IsAbortTriggered = true;
    }
    
    
}
