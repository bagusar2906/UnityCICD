using SimulatorClient;
using UnityEngine;

public class NanoZMotor : MotorBase<NanoAxis>
{

    [SerializeField] public float scale;

    [SerializeField] public float offset;
    // Start is called before the first frame update
    void Start()
    {
        StartUp();
    }

    protected override Vector3 ToEngVector(Vector3 unity)
    {
        return new Vector3(unity.z, (unity.x-offset)/scale, unity.y);
    }

    protected override Vector3 ToUnity(Vector3 engVector)
    {
        return new Vector3( offset + scale * engVector.y, engVector.z, engVector.x);
    }
    public override void MoveAbs(double pos, double vel)
    {
        
        base.MoveAbs(scale*pos + offset, 10*vel);
    }

    // Update is called once per frame
}