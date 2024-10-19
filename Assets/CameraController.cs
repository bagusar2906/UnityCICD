using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{


  private Vector3
    _lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)

  float camSens = 0.25f; //How sensitive it with mouse
  public static CameraController Instance { get; private set; }

  [UsedImplicitly]
  void Start()
  {
    Instance = this;
  }

  [UsedImplicitly]
  void Update()
  {

    _lastMouse = Input.mousePosition - _lastMouse;
    _lastMouse = new Vector3(-_lastMouse.y * camSens, _lastMouse.x * camSens, 0);
    _lastMouse = new Vector3(transform.eulerAngles.x + _lastMouse.x, transform.eulerAngles.y + _lastMouse.y, 0);


    var p = GetBaseInput();
    transform.Translate(p);
    _lastMouse = Input.mousePosition;
  }

  private Vector3 GetBaseInput()
  {

    var pVelocity = new Vector3();
    if (Input.GetMouseButton((int)MouseButton.RightMouse))
    {

      transform.eulerAngles = _lastMouse;

      return pVelocity;
    }

    if (Input.GetKey(KeyCode.W))
    {
      pVelocity += new Vector3(0, 1, 0);
    }

    if (Input.GetKey(KeyCode.S))
    {
      pVelocity += new Vector3(0, -1, 0);
    }

    if (Input.GetKey(KeyCode.A))
    {
      pVelocity += new Vector3(-1, 0, 0);
    }

    if (Input.GetKey(KeyCode.D))
    {
      pVelocity += new Vector3(1, 0, 0);
    }

    pVelocity *= 0.01f;

    pVelocity.z += Input.mouseScrollDelta.y * 0.2f; //zoom in , zoom out

    return pVelocity;
  }

  public void SelectCamera(int cameraView)
  {
    Vector3 rotation, position;
    switch ((CameraView)cameraView)
    {
      case CameraView.Front:
         rotation = new Vector3(0f, 90f, 0f);
         position = new Vector3(-1.16f, -0.03f, -0.33f);
        break;
      case CameraView.Right:
        rotation = new Vector3(0f, 0f, 0f);
        position = new Vector3(0.18f, -0.03f, -3.058f);
        break;
      case CameraView.Back: //Back view
        rotation = new Vector3(0f, -90f, 0f);
        position = new Vector3(1.94f, -0.03f, -0.17f);
        break;
      case CameraView.Left: //Left view
        rotation = new Vector3(0f, 180f, 0f);
        position = new Vector3(0.18f, -0.03f, 1.3f);
        break;
      case CameraView.Top: //Top view
        rotation = new Vector3(90f, 0f, -90f);
        position = new Vector3(0.3f, 1.5f, -0.4f);
        break;
      default:
        rotation = new Vector3(0f, 90f, 0f);
        position = new Vector3(-1.16f, -0.03f, -0.33f);
        break;
    }
    
    transform.localRotation = Quaternion.Euler(rotation);
    if (transform != null) transform.localPosition = position;
  }
}

public enum CameraView
{
   Front,
   Right,
   Back,
   Left,
   Top
}