using System;
using UnityEngine;

public class BarcodeScanner : MonoBehaviour
{
   
    private BarcodeScannerMotor _motor;
    [SerializeField] private GameObject scanner;

    public event EventHandler<string> OnDataReceived; 
    public bool IsDataAvailable { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        IsDataAvailable = false;
        _motor = GetComponentInParent<BarcodeScannerMotor>();
    }

    public void StartScan()
    {
        IsDataAvailable = false;
        _motor.MoveMotor = true;
        scanner.SetActive(true);
        var scanCam = GetComponentInChildren<BarcodeDecoder>();
        scanCam.OnDataReceived = data =>
        {
            OnDataReceived?.Invoke(this, data);
            IsDataAvailable = true;
            _motor.MoveMotor = false;
        }; 
    }
    
    
}