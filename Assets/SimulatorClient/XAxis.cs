using UnityEngine;

namespace SimulatorClient
{
    public class XAxis:IAxis
    {
        public Vector3 Axis => new(1,0,0);
        public Vector3 SetAxisValue(Vector3 vector, float position)
        {
            return new Vector3(position, vector.y, vector.z);
        }
    }
}