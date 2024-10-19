using UnityEngine;

public class TubeAdapter : MonoBehaviour
{
    [SerializeField] public GameObject tubePrefab;

    public void CreateTube(Transform parent, Vector3 localPosition)
    {
        var tube = Instantiate(tubePrefab, Vector3.zero, Quaternion.identity);
        tube.transform.SetParent(parent);
        tube.transform.localPosition = localPosition;
    }
}