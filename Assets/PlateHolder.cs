using UnityEngine;
using UnityEngine.UIElements;

public class PlateHolder : MonoBehaviour
{
    private readonly float _dropRange = 0.51f;
    // Update is called once per frame
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.tag.StartsWith("DropObject"))
            return;
        var dropObj = collision.collider;
     //   dropObj.transform.SetParent(transform);
     //   dropObj.transform.parent = transform;
        var objRb = dropObj.GetComponent<Rigidbody>();
        objRb.isKinematic = true;
        objRb.useGravity = false;
        objRb.constraints = RigidbodyConstraints.FreezeRotation;
        
    }

    
    void Update()
    {
        if (transform.parent.CompareTag("DropArea"))
            return;
        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out var hit,
                _dropRange)) return;
        if (!hit.collider.CompareTag("DragObject")) return;
        var draggedObject = hit.collider.gameObject;

        if (!Input.GetMouseButtonDown((int)MouseButton.LeftMouse)) return;
       // if (FindTag(transform, "DragObject"))
       //     return; //already has dragged object
        
        draggedObject.transform.SetParent(transform);
        draggedObject.transform.localPosition = new Vector3(0f, 0.4f, 0f);
        //(transform1 = transform).SetParent(draggedObject.transform);
        //transform1.localPosition = new Vector3(0f, 0.02f, 0f);
        //transform.position = new Vector3(0f, 0.01f, 0f);
        // PickupObject(hit.transform.gameObject);
    }

    public static bool FindTag(Component parent, string childTag)
    {
        foreach(Transform childTransform in parent.transform) {
            if(childTransform.CompareTag(childTag)) {
               var  child = childTransform.gameObject;
                return true;
            }
        }

        return false;
        // return parent.transform.Cast<Transform>().Any(childTransform => childTransform.CompareTag(childTag));
    }
}
