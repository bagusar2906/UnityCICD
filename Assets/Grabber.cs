using UnityEngine;
using UnityEngine.UIElements;

public class Grabber : MonoBehaviour
{
    private GameObject _selectedObject;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
        {
            if (_selectedObject == null)
            {
                var hit = CastRay();
                if (hit.collider != null && !hit.collider.CompareTag("DragObject"))
                    return;
                if (hit.collider == null)
                    return;
                _selectedObject = hit.collider.gameObject;
            }
            else
            {
                DropObject();

            }
            
                
        }

        if (_selectedObject != null)
        {
            DragObject();
        }
    }

    private void DragObject()
    {
        var position = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            Camera.main.WorldToScreenPoint(_selectedObject.transform.position).z);
        var worldPosition = Camera.main.ScreenToWorldPoint(position);
        _selectedObject.transform.position = new Vector3(worldPosition.x, 0.6f, worldPosition.z);
    }
    
    private void DropObject()
    {

        var position = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            Camera.main.WorldToScreenPoint(_selectedObject.transform.position).z);
        var worldPosition = Camera.main.ScreenToWorldPoint(position);
        _selectedObject.transform.position = new Vector3(worldPosition.x, 0.40f, worldPosition.z);
        _selectedObject = null;
        
    }

    RaycastHit CastRay()
    {
        var screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        var screenMousePosNear =new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);

        var wordMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        var wordMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        Physics.Raycast(wordMousePosNear, wordMousePosFar - wordMousePosNear, out var hit);
        return hit;
    }
}
