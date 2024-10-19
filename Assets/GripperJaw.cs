using System;
using System.Threading;
using JetBrains.Annotations;
using SimulatorClient;
using SimulatorClient.EventArgs;
using UnityEngine;
using UnityEngine.Serialization;

public class GripperJaw : MotorBase<YAxis>
{
    public override void MoveAbs(double pos, double vel)
    {
        //speed up gripper movement
        vel = 2 * vel;
        base.MoveAbs(pos, vel);
    }

    public event EventHandler<GripperCurrentChangedEventArgs> OnHoldingCurrentChanged;
    private bool _shouldMove;

    private Collider HeldObj { get; set; }

    [FormerlySerializedAs("HSpeed")] public float hSpeed = 3f;

    [FormerlySerializedAs("Scale")] public float scale = -1;
    [FormerlySerializedAs("Offset")] public float offset = 0;
    
    private Vector3 _origPosition;



    private SynchronizationContext _syncContext;
    private Renderer _componentRender;


    public bool IsClosed { get; set; }

    private Color _unClampColor;
    private Color _clampColor = Color.red;

    [UsedImplicitly]
    // Start is called before the first frame update
    void Start()
    {
        StartUp();      
        _componentRender = GetComponent<Renderer>();
        _unClampColor =  _componentRender.material.color ;
    }


    protected override Vector3 ToEngVector(Vector3 unity)
    {
        return new Vector3(unity.z, (unity.x-offset)/scale, unity.y);
    }

    protected override Vector3 ToUnity(Vector3 engVector)
    {
        return new Vector3( offset + scale * engVector.y, engVector.z, engVector.x);
    }
    
    private void OnTriggerEnter(Collider colliderObj)
    {

        if (!colliderObj.tag.StartsWith("DropObject")
            && !colliderObj.tag.StartsWith("Chip"))
            return;
        if (!IsMoving)
            return;
        if (transform.childCount > 0)
            return;

        _componentRender.material.color = Color.red;
        StopMove();
        IsTouching = true;
        HeldObj = colliderObj;
        OnHoldingCurrentChanged?.Invoke(this, new GripperCurrentChangedEventArgs()
        {
            MotorID = Id,
            HoldingCurrent = 500
        });
        var gripperMotor= GetComponentInParent<GripperMotor>();
        HeldObj.transform.SetParent(gripperMotor.transform.parent);
        //     Grippers[0].HeldObj.transform.localPosition = new Vector3(0f, -8.5f, 0f);
        Debug.Log($"Gripper is touching");
        var objRb = HeldObj.GetComponent<Rigidbody>();
        if (objRb == null)
            return;
        objRb.isKinematic = true;
        objRb.useGravity = false;
       // objRb.constraints = RigidbodyConstraints.FreezeRotation;

    }

    private void OnTriggerExit(Collider colliderObj)
    {
        
        if (!colliderObj.tag.StartsWith("DropObject")
            && !colliderObj.tag.StartsWith("Chip"))
            return;
        if (!IsMoving)
            return;
        
       
        
        if (HeldObj == null)
            return;

        var objRb = HeldObj.GetComponent<Rigidbody>();
        if (objRb == null)
            return;
        Debug.Log($"Gripper is un touching");
        objRb.isKinematic = false;
        objRb.useGravity = true;
     //   objRb.constraints = RigidbodyConstraints.None;
        HeldObj.transform.parent = null;
        var grippedObject = HeldObj.GetComponent<GrippedObject>();
        grippedObject.IsHandedOver = true;
        HeldObj = null;
        IsTouching = false;
        OnHoldingCurrentChanged?.Invoke(this, new GripperCurrentChangedEventArgs()
        {
            MotorID = Id,
            HoldingCurrent = 1
        });
        _componentRender.material.color = _unClampColor;

    }

    //in close position, the target pos never be
    //the same with actual pos because the move would be stopped once the jaw touch the tube
    public GripperState CurrentState
    {
        get
        {
            var state = Math.Abs(InitialPosition) < Math.Abs(TargetPosition) 
                ? IsMoving ? GripperState.Closing: GripperState.Closed
                : IsMoving ? GripperState.Opening: GripperState.Opened;
            return state;
        }

    }

    [Flags]
    public enum GripperState: ushort
    {
        Opened,
        Closing,
        Closed,
        Opening
    }


 
}
