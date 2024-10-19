using JetBrains.Annotations;
using SimulatorClient;
using UnityEngine;

public class Laser : MonoBehaviour, IStopInput
{
  private LineRenderer _lr;
  private Renderer _componentRender;
  private double _prevDistance;

  public static Laser Instance { get; private set; }

  public double Distance { get; private set; }

  public bool Activated
  {
    get => _componentRender.enabled;
    set
    {
      if (value )
        Distance = 0;
      
      _componentRender.enabled = value;
    }
  }

  public bool IsAbortTriggered { get; private set; }
  
  public double StopThreshold { get; set; }
  public void Clear()
  {
    IsAbortTriggered = false;
    Distance = 0;
  }

  // Start is called before the first frame update
  [UsedImplicitly]
  void Start()
  {
    _lr = GetComponent<LineRenderer>();
    _componentRender = GetComponent<Renderer>();
    Instance = this;
    Activated = false;
  }

  // Update is called once per frame
  [UsedImplicitly]
  void Update()
  {
    _lr.SetPosition(0, transform.position);
    if (Physics.Raycast(transform.position, transform.forward, out var hit))
    {
      if (!hit.collider) return;
      _lr.SetPosition(1, hit.point);


      _prevDistance = Distance;
      Distance = (transform.position - hit.point).sqrMagnitude * 1000;
      if (!Activated ) return;
      if (StopThreshold == 0)
        return;
      if (_prevDistance == 0) return; // first start
      if (!(System.Math.Abs(_prevDistance - Distance) > StopThreshold)) return;
      IsAbortTriggered = true;
     // Activated = false;
      //print("Distance: " + Distance.ToString("F3"));
    }
    else
    {
      _lr.SetPosition(1, transform.forward*500);
    }
  }
}