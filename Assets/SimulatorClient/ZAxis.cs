using UnityEngine;

namespace SimulatorClient
{
    public class ZAxis:IAxis
    {
        public Vector3 Axis => new(0,0,1);
        public Vector3 SetAxisValue(Vector3 vector, float position)
        {
            return new Vector3(vector.x, vector.y, position);
        }
    }
}