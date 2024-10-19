using UnityEngine;using UnityEngine.UIElements;


public class PickupController : MonoBehaviour
{
    [Header("Pickup Settings")] [SerializeField]
    private Transform holdArea;

    private GameObject heldObj;
    private Rigidbody heldObjRB;
    
    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 5f;

    [SerializeField] private float pickupForce = 150f;
 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
        {
            if (heldObj == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    PickupObject(hit.transform.gameObject);
                }
            }
            else
            {
                DropObject();
            }
        }

        if (heldObj != null)
        {
            MoveObject();
        }
        
    }

    private void MoveObject()
    {
        if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
        {
            var moveDirection = holdArea.position - heldObj.transform.position;
            heldObjRB.AddForce(moveDirection * pickupForce);
        }
    }

    private void DropObject()
    {
        heldObjRB.useGravity = true;
        heldObjRB.drag = 1;
        heldObjRB.constraints = RigidbodyConstraints.None;

        heldObjRB.transform.parent = null;
        heldObj = null;
    }

    private void PickupObject(GameObject pickObj)
    {
        if (pickObj.GetComponent<Rigidbody>())
        {
            heldObjRB = pickObj.GetComponent<Rigidbody>();
            heldObjRB.useGravity = false;
            heldObjRB.drag = 10;
            heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;

            heldObjRB.transform.parent = holdArea;
            heldObj = pickObj;
        }
    }
}
