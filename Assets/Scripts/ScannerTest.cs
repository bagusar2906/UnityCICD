using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerTest : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] private GameObject scannerCam;

    private void Start()
    {
        var codeGenerator = GetComponentInChildren<QRCodeGenerator>();
        codeGenerator.ApplyTubeBarcode( TubeType .Tube_50mL,"AAABBBB");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           // mainCam.SetActive(false);
            scannerCam.SetActive(true);
        }
    }
    
}
