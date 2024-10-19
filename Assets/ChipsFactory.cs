using System.Collections.Generic;
using System.Linq;
using SimulatorClient.Extensions;
using SimulatorClient.Protocol;
using UnityEngine;
using UnityEngine.Serialization;

public class ChipsFactory : MonoBehaviour
{
    [FormerlySerializedAs("NumOfChips")]
    public int numOfChips;

    [FormerlySerializedAs("Pitch")] 
    public float pitch;
    // Start is called before the first frame update

    [SerializeField] public GameObject chip;

    [SerializeField] public Vector3 origin;
    void Start()
    {
        CreateShafts(numOfChips);
    }

    public void CreateChips(int deckId, IEnumerable<ChipDto> chips, GameObject rackGameObject)
    {
      //  var isFirst = true;
      //  var original =transform.Find("Chip-A1").gameObject;
    //    var localPosition = original.transform.localPosition;
   //     var originPos = new Vector3(localPosition.x, localPosition.y, localPosition.z);
        var prefix = "Chip";
       // var chipGameObject = original;
       foreach (var chipDto in chips.Where(t => t.chipSlot.deckId == deckId && t.mwco > 0))
       {


           // chipGameObject = Instantiate(original, transform, true);
           // Instantiate at position (0, 0, 0) and zero rotation.
           var chipGameObject = Instantiate(chip, origin, Quaternion.identity);
           var barcodeContent = chipGameObject.GetComponentInChildren<QRCodeGenerator>();
           barcodeContent.ApplyChipBarcode("Test");
           chipGameObject.transform.Rotate(0, 180, 0);
           chipGameObject.transform.SetParent(rackGameObject.transform);
           PlaceChip(prefix, chipDto.chipSlot.slot, chipGameObject, origin);
           var chipContent = chipGameObject.GetComponentInChildren<Chip>();
           chipContent.PrintLabel(chipDto.mwco);

       }
    }

    private void CreateShafts(int chipsNum)
    {

        var original = GameObject.Find("ChipShaft");
        var localPosition = original.transform.localPosition;
        var originPos = new Vector3(localPosition.x, localPosition.y, localPosition.z);
        
        for (var i = 1; i < chipsNum; i++)
        {
            var shaftClone = Instantiate(original, transform, true);
            var well = $"A{i + 1}";
            var prefix = "Shaft";
            PlaceChip(prefix, well,  shaftClone, originPos);
        }

    }

    private void PlaceChip(string prefix,  string well, GameObject chipClone, Vector3 localPosition)
    {
        var colRow = well.ToColRow();
        var row = colRow[1];
        chipClone.name = prefix + well;
        
        chipClone.transform.localPosition = new Vector3(localPosition.x,
            localPosition.y, localPosition.z - row * pitch);
    }
}
