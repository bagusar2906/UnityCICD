using UnityEngine;

public static class CustomExtensions
{
  public static Vector3 ToUnity(this Vector3 engVector)
  {
    return new Vector3(engVector.y, 200f - engVector.z, engVector.x)/1000;
  }

  public static Vector3 ToEngVector(this Vector3 unity)
  {
    return new Vector3(unity.z, unity.x, 0.2f - unity.y) * 1000;
  }
}