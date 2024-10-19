using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class CameraSelection : MonoBehaviour
{
  // Start is called before the first frame update
  [UsedImplicitly]
  public void OnSelectCamera()
  {
    var cam = CameraController.Instance;
    var dropDown = GetComponent<Dropdown>();
    cam.SelectCamera(dropDown.value);
  }
}