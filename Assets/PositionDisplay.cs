using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

[UsedImplicitly]
public class PositionDisplay : MonoBehaviour
{
  // Start is called before the first frame update
  private IDictionary<string, Text> _labels;
  private bool _isInstanceNull;

  [UsedImplicitly]
  void Start()
  {
    _isInstanceNull = Simulator.Instance == null;
    _labels = new Dictionary<string, Text>();
    foreach (var label in GetComponentsInChildren<Text>())
    {
      _labels[label.name] = label;
    }
  }

  // Update is called once per frame
  [UsedImplicitly]
  void Update()
  {
    if (_isInstanceNull)
      return;

    var sim = Simulator.Instance;
    _labels["posX"].text = sim.CurrentPosition.x.ToString("F2");
    _labels["posY"].text = sim.CurrentPosition.y.ToString("F2");
    _labels["posZ"].text = sim.CurrentPosition.z.ToString("F2");

  }
}