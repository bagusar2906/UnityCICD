using UnityEngine;

public class LiquidControl : MonoBehaviour
{
    
    [SerializeField] public float volume;
    [SerializeField] public float offset;
    [SerializeField] public float scale;
    [SerializeField] public Color liquidColor;
    [SerializeField] public Color surfaceColor;
    
    private static readonly int Fill = Shader.PropertyToID("_Fill");
    private static readonly int LiquidColor = Shader.PropertyToID("_LiquidColor");
    private static readonly int SurfaceColor = Shader.PropertyToID("_SurfaceColor");
    private IMaterialLiquid _liquidMaterial;
    

    private void SetLiquidVolume()
    {
        SetVolume(volume);
    }
    
    public void SetVolume(LiquidType liquidType,  float liquidVolume)
    {
       _liquidMaterial = liquidType == LiquidType.Sample ?  new SampleLiquid(LiquidColor, SurfaceColor) 
            : new BufferLiquid(LiquidColor, SurfaceColor);
        volume = liquidVolume;
        var fillLevel = offset + scale * liquidVolume;
        var mPropertyBlock = _liquidMaterial.MaterialPropertyBlock;
        mPropertyBlock.SetFloat(Fill, fillLevel);
        // mPropertyBlock.SetColor(LiquidColor, liquidColor);
        // mPropertyBlock.SetColor(SurfaceColor, surfaceColor);
        var myRenderer = GetComponentInChildren<Renderer> ();
        myRenderer.SetPropertyBlock(mPropertyBlock);
    }

    public void SetVolume(float liquidVolume)
    {
        volume = liquidVolume;
        var fillLevel = offset + scale * liquidVolume;
        _liquidMaterial ??= new BufferLiquid(LiquidColor, SurfaceColor);
        var mPropertyBlock = _liquidMaterial.MaterialPropertyBlock;
        mPropertyBlock.SetFloat(Fill, fillLevel);
        // mPropertyBlock.SetColor(LiquidColor, liquidColor);
        // mPropertyBlock.SetColor(SurfaceColor, surfaceColor);
        var myRenderer = GetComponentInChildren<Renderer> ();
        myRenderer.SetPropertyBlock(mPropertyBlock);
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
            return;
        SetLiquidVolume();
    }
}