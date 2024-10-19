using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

[UsedImplicitly]
public class ProbeButton : MonoBehaviour
{
  private Text _onOffLabel;

  void Start()
  {
    _onOffLabel = GetComponentsInChildren<Text>()[0];
  }

  [UsedImplicitly]
  public void ToggleOnOff()
  {
    var probe = Probe.Instance;
    if (probe.IsLaserOn)
    {
      probe.SwitchLaserOff();
      _onOffLabel.text = "Turn laser ON";
    }
    else
    {
      probe.SwitchLaserOn();
      _onOffLabel.text = "Turn laser OFF";
    }
  }
}