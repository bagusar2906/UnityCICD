using System;
using UnityEngine;
using ZXing;

[RequireComponent(typeof(Camera))]
public class BarcodeDecoder : MonoBehaviour
{
    private Camera _snapCam;

    private int _resWidth = 256;

    private int _resHeight = 256;

    private Texture2D _snapshot;

    public Action<string> OnDataReceived;
    // Start is called before the first frame update
    void Awake()
    { 
       
        _snapCam = GetComponent<Camera>();
        if (_snapCam.targetTexture == null)
        {
            _snapCam.targetTexture = new RenderTexture(_resWidth, _resHeight, 24);
        }
        else
        {
            var targetTexture = _snapCam.targetTexture;
            _resHeight = targetTexture.height;
            _resWidth = targetTexture.width;
        }

    }
    
    private void LateUpdate()
    {
        
        if (!_snapCam.gameObject.activeInHierarchy) return;
        _snapshot = new Texture2D(_resWidth, _resHeight, TextureFormat.RGB24, false);
        _snapCam.Render();
        RenderTexture.active = _snapCam.targetTexture;
        _snapshot.ReadPixels(new Rect(0, 0, _resWidth, _resHeight), 0, 0);
        IBarcodeReader barcodeReader = new BarcodeReader();
        var result = barcodeReader.Decode(_snapshot.GetPixels32(), _snapshot.width, _snapshot.height);
        if (result == null) return;
        Debug.Log("Barcode Text: " + result.Text);
        OnDataReceived?.Invoke(result.Text);
        _snapCam.gameObject.SetActive(false);
    }
    
    /*void OnGUI()
    {
      //  if (_snapshot == null) return;
        var screenRect = new Rect(0, 0, Screen.width, Screen.height);
        GUI.DrawTexture(screenRect, _snapshot , ScaleMode.ScaleToFit);
    }*/
}