using UnityEngine;

namespace SimulatorClient
{
    public interface IAxis
    {
        Vector3 Axis { get; }
        Vector3 SetAxisValue(Vector3 vector, float position);
    }
}