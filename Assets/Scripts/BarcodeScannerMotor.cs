using UnityEngine;

public class BarcodeScannerMotor : MonoBehaviour
{
    
    public bool MoveMotor 
    {
        set
        {
            var joint = GetComponent<HingeJoint>();
            joint.useMotor = value;
        }

        get
        {
            var joint = GetComponent<HingeJoint>();
            return joint.useMotor;
        }
    }
    
}