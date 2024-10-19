using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

[UsedImplicitly]
public class DistanceDisplay : MonoBehaviour
{
  private Text _distanceText;

  // Start is called before the first frame update
  [UsedImplicitly]
  void Start()
  {
    _distanceText = GetComponentsInChildren<Text>()[0];
  }

  // Update is called once per frame
  [UsedImplicitly]
  void Update()
  {
    if (Probe.Instance.IsLaserOn)
      _distanceText.text = Probe.Instance.DistanceMeasure.ToString("F3");
  }
}