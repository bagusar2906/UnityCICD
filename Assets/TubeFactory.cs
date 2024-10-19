using System;
using System.Collections.Generic;
using System.Linq;
using SimulatorClient.Extensions;
using SimulatorClient.Protocol;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TubeFactory : MonoBehaviour
{
    // Start is called before the first frame update
    [FormerlySerializedAs("Rows")]
    public int rows;
    [FormerlySerializedAs("Columns")] 
    public int columns;

    [FormerlySerializedAs("Column Pitch")]
    public float columPitch;
    [FormerlySerializedAs("Row Pitch")]
    public float rowPitch;

    [SerializeField] public GameObject tubePrefab;
    [SerializeField] public Vector3 origin;
    void Start()
    {
        CreateWells(columns, rows, columPitch, rowPitch);
    }


    public void CreateTubes(int deckId, IEnumerable<TubeDto> tubesData, GameObject rackGameObject)
    {
        var random = new System.Random();
        foreach (var tube in tubesData.Where(t => t.tubeSlot.deckId == deckId 
                                                  && !t.liquidType.StartsWith("NoTube")))
        {
            var tubeGameObject = Instantiate(tubePrefab, origin, Quaternion.identity);
            var tubeContent = tubeGameObject.GetComponentInChildren<Tube>();
            var barcodeContent = tubeGameObject.GetComponentInChildren<QRCodeGenerator>();
            barcodeContent.ApplyTubeBarcode(tubeContent.tubeType, "Test");
            tubeGameObject.transform.Rotate(0, random.Next(1, 360), 0);
            tubeGameObject.transform.SetParent(rackGameObject.transform);
            PlaceTube(origin, tube.tubeSlot.slot, columPitch, rowPitch, tubeGameObject);
            if (!Enum.TryParse(typeof(LiquidType), tube.liquidType, true, out var liquidType)) 
                continue;
            liquidType = tubeContent.tubeType == TubeType.Tube_1_5mL ? LiquidType.Sample : liquidType;
            tubeContent.Fill((LiquidType)liquidType, (float)tube.startVolume);
         
        }
    }

   

    private void CreateWells(int colNum, int rowNum, double pitchCol, double pitchRow)
    {
        var colNames = new[] {"A","B","C","D","E","F","G","H"};

        var tubeSlot = transform.Find("TubeSlot").gameObject;
        for (var col = 0 ; col < colNum; col++)
        {

            for (var row = 0; row < rowNum; row++)
            {
                if (col == 0 && row == 0)
                {
                   
              
                    CreateWell("Slot", pitchCol, pitchRow, tubeSlot, colNames, col, row);
                    
                }
                else
                {
                    var     cloneGameObject = Instantiate(tubeSlot, transform, true);
                    CreateWell("Slot", pitchCol, pitchRow, cloneGameObject, colNames, col, row);
                }

            }
        }

    }

    private static void CreateWell(string prefixName, double pitchCol, double pitchRow, GameObject gameObject, IReadOnlyList<string> colNames, int col,
        int row)
    {
        gameObject.name = $"{prefixName}-{colNames[col]}{row + 1}";
        var localPosition = gameObject.transform.localPosition;
        gameObject.transform.localPosition = new Vector3((float)(localPosition.x - col * pitchCol),
            localPosition.y, (float)(localPosition.z - row * pitchRow));
    }
    
    private static void PlaceTube(Vector3 originPos, string well, double pitchCol, double pitchRow,
        GameObject gameObject)
    {
        var colRow = well.ToColRow();
        var col = colRow[0];
        var row = colRow[1];
        gameObject.name = $"Tube-{well}";
        gameObject.transform.localPosition = new Vector3((float)(originPos.x - col * pitchCol),
            originPos.y, (float)(originPos.z - row * pitchRow));
    }
    
}