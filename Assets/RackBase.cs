using UnityEngine;

public class RackBase : MonoBehaviour
{
    // Start is called before the first frame update

    protected string ObjectTag { get; set; }

    private void Start()
    {
        ObjectTag = "DropObject";
    }
    
    
    private Collider _placedObject;
   
    private void OnCollisionEnter(Collision collision)
    {
        if (string.IsNullOrEmpty(ObjectTag))
            return;

        if (!collision.collider.CompareTag(ObjectTag))
            return;
        _placedObject = collision.collider;
        var objRb = _placedObject.GetComponent<Rigidbody>();
        objRb.isKinematic = true;
        objRb.useGravity = false;
        
    }

    
    private void OnCollisionExit(Collision other)
    {
        if (!other.collider.CompareTag(ObjectTag))
            return;
        _placedObject = null;
    }

    private void Update()
    {
        if (_placedObject == null) return;
        var grippedObject = _placedObject.GetComponent<GrippedObject>();
        if (!grippedObject.IsHandedOver) return;
        (_placedObject.transform).SetParent(transform);
        
        grippedObject.IsHandedOver = false;
    }

    
}