using System;
using SimulatorClient.Enums;
using UnityEngine.Serialization;

[Serializable]
public class StationConfig
{
    public short id;
    public StationTypeEnum type;
    public short busId;
}