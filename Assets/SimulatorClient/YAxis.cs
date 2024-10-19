using UnityEngine;

namespace SimulatorClient
{
    public class YAxis:IAxis
    {
        public Vector3 Axis => new(0,1,0);
        public Vector3 SetAxisValue(Vector3 vector, float position)
        {
            return new Vector3(vector.x, position, vector.z);
        }
    }
}