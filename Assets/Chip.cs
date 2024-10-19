using TMPro;
using UnityEngine;

public class Chip : MonoBehaviour
{
   public void PrintLabel(int mwco)
   {
      var mwcoLabel = GetComponentInChildren<TextMeshPro>();
      mwcoLabel.text = $"{mwco} kda";
   }
}
