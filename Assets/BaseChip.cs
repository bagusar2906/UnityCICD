using UnityEngine;

public class BaseChip : MonoBehaviour
{
    // Start is called before the first frame update

    private const string ObjectTag = "Chip";


    private Collider _placedObject;

    private void Update()
    {
        if (_placedObject == null) return;
        var grippedObject = _placedObject.GetComponent<GrippedObject>();
        if (!grippedObject.IsHandedOver) return;
        Transform transform1;
        (transform1 = _placedObject.transform).SetParent(transform);

     //   transform1.localPosition = grippedObject.WellTubeLocation;
     //   var localPos = grippedObject.WellTubeLocation.ToEngVector();
    //    print($"Placed at location: ({localPos.x}, {localPos.y}, {localPos.z}) ");

        grippedObject.IsHandedOver = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(ObjectTag))
            return;
        var grippedObject = other.GetComponent<GrippedObject>();
        if (grippedObject == null)
            return;
        _placedObject = other;
    }

    private void OnTriggerExit(Collider other)
    {
        _placedObject = null;
    }
}