using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;

public class QRCodeGenerator : MonoBehaviour
{
    private readonly IDictionary<TubeType, Vector3> _barcodePosition = new Dictionary<TubeType, Vector3>()
    {
        { TubeType.Tube_1_5mL,  new Vector3(0f, -0.01f, -0.009f) },
        { TubeType.Tube_15mL,  new Vector3(0f, -0.03f, -0.0129f) },
        { TubeType.Tube_50mL,  new Vector3(0f, -0.03f, -0.008f) }
    };
    public void ApplyTubeBarcode(TubeType tubeType, string barcodeText)
    {
        var barcodeTexture = GenerateBarcodeTexture(barcodeText, 256, 256);
        // Apply the texture to a material
        //"DummyPipeline/VariantStrippingTestsShader"
        var material = new Material(Shader.Find("Sprites/Default"))
        {
            mainTexture = barcodeTexture
        };
        //  ApplyBarcodeTexture(barcodeTexture);
        var barcodeObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        var meshCollider = barcodeObject.GetComponentInChildren<MeshCollider>();
        //no need collider and if not removed, will get warning
        if (meshCollider != null)
            DestroyImmediate(meshCollider, true);
        barcodeObject.transform.parent = transform;
        barcodeObject.transform.localPosition = _barcodePosition[tubeType];
        barcodeObject.transform.localScale = new Vector3(0.01f, 0.02f, 1f);

        // Apply the material to the GameObject
        var component = barcodeObject.GetComponent<Renderer>();
        component.material = material;
    }
    
    public void ApplyChipBarcode(string barcodeText)
    {
        var barcodeTexture = GenerateBarcodeTexture(barcodeText, 256, 256);
        // Apply the texture to a material
        //"DummyPipeline/VariantStrippingTestsShader"
        var material = new Material(Shader.Find("Sprites/Default"))
        {
            mainTexture = barcodeTexture
        };
        //  ApplyBarcodeTexture(barcodeTexture);
        var barcodeObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        var meshCollider = barcodeObject.GetComponentInChildren<MeshCollider>();
        //no need collider and if not removed, will get warning
        if (meshCollider != null)
            DestroyImmediate(meshCollider, true);
        barcodeObject.transform.parent = transform;
        barcodeObject.transform.localPosition = new Vector3(0.06f, 0.50f, 0);
        barcodeObject.transform.localScale = new Vector3(0.125f, 0.77f, 1f);
        barcodeObject.transform.Rotate(90, 0, 0);

        // Apply the material to the GameObject
        var component = barcodeObject.GetComponent<Renderer>();
        component.material = material;
    }

    private Texture2D GenerateBarcodeTexture(string data, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.CODE_128,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };

        var pixels = writer.Write(data);
        pixels = RotateMatrix(pixels, width);
        var texture = new Texture2D(width, height);
        texture.SetPixels32(pixels);
        texture.Apply();

        return texture;
    }
  
    static Color32[] RotateMatrix(Color32[] matrix, int n) {
        Color32[] ret = new Color32[n * n];
		
        for (int i = 0; i < n; ++i) {
            for (int j = 0; j < n; ++j) {
                ret[i*n + j] = matrix[(n - j - 1) * n + i];
            }
        }
		
        return ret;
    }
  
}
