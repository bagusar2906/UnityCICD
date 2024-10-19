using UnityEngine;

public interface IMaterialLiquid
{
    MaterialPropertyBlock MaterialPropertyBlock { get; }
}

public class SampleLiquid : IMaterialLiquid
{
    public SampleLiquid(int liquidShaderId, int surfaceShaderId)
    {
        MaterialPropertyBlock = new MaterialPropertyBlock();
        if (ColorUtility.TryParseHtmlString("#DE1EE9", out var color))
            MaterialPropertyBlock.SetColor(liquidShaderId, color);
        if (ColorUtility.TryParseHtmlString("#CF36B0", out var surfaceColor))
            MaterialPropertyBlock.SetColor(surfaceShaderId, surfaceColor);
    }

    public MaterialPropertyBlock MaterialPropertyBlock { get; }
}
