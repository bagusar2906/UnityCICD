using UnityEngine;

public class LedIndicator : MonoBehaviour
{
    private Renderer _componentRender;

    // Start is called before the first frame update
    void Start()
    {
        _componentRender = GetComponent<Renderer>();
        _componentRender.material.color = PulseState.IDLE;
    }

    public Color State
    {
        get => _componentRender.material.color;
        set => _componentRender.material.color = value;
    }

    
}