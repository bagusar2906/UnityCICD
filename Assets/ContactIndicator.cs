using UnityEngine;

public class ContactIndicator : MonoBehaviour
{
  // Start is called before the first frame update

  private readonly Color OnContactColor = Color.red;
  private Renderer _componentRender;
  void Start()
  {
    _componentRender = GetComponent<Renderer>();
  }

  // Update is called once per frame
  void Update()
  {
    if (Probe.Instance == null)
      return;

    _componentRender.material.color = Probe.Instance.IsTouching ? OnContactColor : Color.green;
  }
}