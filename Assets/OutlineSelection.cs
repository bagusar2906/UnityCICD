using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class OutlineSelection : MonoBehaviour
{
    private Transform highLight;

    private Transform selection;

    private RaycastHit raycastHit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Highlight

        if (highLight != null)
        {
            highLight.gameObject.GetComponent<Outline>().enabled = false;
            highLight = null;
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out  raycastHit))
        {
            highLight = raycastHit.transform;
            if (highLight.CompareTag("Selectable") ||
                highLight.CompareTag("DropObject") || highLight.CompareTag("DragObject"))
            {
                if (highLight.gameObject.GetComponent<Outline>() != null)
                {
                    highLight.GetComponent<Outline>().enabled = true;
                }
                else
                {
                    var outline = highLight.gameObject.AddComponent<Outline>();
                    outline.enabled = true;
                    outline.OutlineColor = Color.cyan;
                    outline.OutlineWidth = 2.0f;
                }
            }
            else
            {
                highLight = null;
            }
        }
        
        //Selection

        if (!Input.GetMouseButtonDown((int)MouseButton.RightMouse)) return;
        if (highLight)
        {
            if (selection != null)
            {
                selection.gameObject.GetComponent<Outline>().enabled = false;
            }

            selection = raycastHit.transform;
            selection.gameObject.GetComponent<Outline>().enabled = true;
            highLight = null;
        }
        else
        {
            if (!selection) return;
            selection.gameObject.GetComponent<Outline>().enabled = false;
            selection = null;
        }
    }
}
