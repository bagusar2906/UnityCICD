using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCollision : MonoBehaviour
{

    [SerializeField] public GameObject collisionSensor;
    // Start is called before the first frame update

    public void OnTriggerCollisionClick()
    {
        var comp = collisionSensor.GetComponent<CollisionSensor>();
        comp.TriggerCollision();

    }
}
