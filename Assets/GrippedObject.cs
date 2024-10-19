using UnityEngine;

public class GrippedObject : MonoBehaviour
{
   private void Start()
   {
      IsHandedOver = false;
   }

   // true if handed over by gripper
   public bool IsHandedOver { get; set; }
 
}
