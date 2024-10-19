using JetBrains.Annotations;
using UnityEngine;

public class Probe : MonoBehaviour
{
  // Start is called before the first frame update
  public static Probe Instance;

  public bool IsTouching => Enabled && _isCollide;

  private float _speed ;
  private bool _isCollide;

  private float _minLimit;
  private bool _shouldExtend;
  private Renderer _componentRender;
  private Color _compColor;

  private bool _enabled;
  public bool Enabled
  {
    get => _enabled;
    set
    {
      if (!Laser.Instance.Activated)
        Laser.Instance.Activated = true;
      _enabled = value;
    }
  }

  public void SwitchLaserOn()
  {
    Laser.Instance.Activated = true;
  }
  public void SwitchLaserOff()
  {
    Laser.Instance.Activated = false;
  }

  public bool IsLaserOn => Laser.Instance.Activated;


  [UsedImplicitly]
  void Start()
  {
    Instance = this;
    _componentRender = GetComponent<Renderer>();
    _compColor = _componentRender.material.color;
      
  }

  // Update is called once per frame
  [UsedImplicitly]
  void Update()
  {
     
    _isCollide = Laser.Instance.Distance < 0.008;
    //if (_isCollide)
    //  print(" Hit distance: " + Laser.Instance.Distance.ToString("F3"));
  }

  public bool IsMoving { get; private set; }
  public double DistanceMeasure => Laser.Instance.Distance;

  void OnTriggerExit(Collider collider)
  {
    if (collider.name.StartsWith("probe_base"))
      return;

    _isCollide = false;
    _componentRender.material.color = _compColor;
  }

  void OnTriggerEnter(Collider collider)
  {
    if (!Enabled)
      return;

    if (collider.name.StartsWith("probe_base"))
      return;
    _isCollide = true;
    _componentRender.material.color = Color.red;

  }
}