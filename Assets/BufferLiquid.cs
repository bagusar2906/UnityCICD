using UnityEngine;


public class BufferLiquid : IMaterialLiquid
{
    public BufferLiquid(int liquidShaderId, int surfaceShaderId)
    {
        MaterialPropertyBlock = new MaterialPropertyBlock();
        if (ColorUtility.TryParseHtmlString("#1A8182", out var color))
            MaterialPropertyBlock.SetColor(liquidShaderId, Color.cyan);
        if (ColorUtility.TryParseHtmlString("#1FBEE7", out var surfaceColor))
            MaterialPropertyBlock.SetColor(surfaceShaderId, Color.cyan);
    }

    public MaterialPropertyBlock MaterialPropertyBlock { get; }
}