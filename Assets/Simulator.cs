using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MultiSamplesPulse;
using SimulatorClient;
using SimulatorClient.Enums;
using SimulatorClient.Protocol;
using UnityEngine;

public class Simulator : MonoBehaviour
{
    private IDictionary<string, GameObject> _rackPrefabsMap;
    public static readonly Vector3 MaxTravelLimit = new(7500f, 6000f, 170.0f);
    public static readonly Vector3 MinTravelLimit = new(0f, -7200f, -1540f);
    public static readonly Vector3 ManualControlSpeed = new Vector3(50f, 50f, 10f);
    // ReSharper disable once InconsistentNaming


    /*[FormerlySerializedAs("MsgBox")] public GameObject msgBox;
    MessageBox _box;*/
    public Simulator()
    {
        SpeedFactor = 1;
    }

    public static Simulator Instance { get; private set; }

    [SerializeField] public GameObject chipCaddy;
    [SerializeField] public GameObject tubeRack50;
    [SerializeField] public GameObject tubeRack15;
    [SerializeField] public GameObject tubeRackLV;
    [SerializeField] public GameObject laptop;
    [SerializeField] public GameObject dashboard;
    [SerializeField] public GameObject barcodeScanner;

    // In millie meter unit
    public Vector3 CurrentPosition => new((float)XAxisMotor.Instance.CurrentPos, (float)YAxisMotor.Instance.CurrentPos,
        (float)ZAxisMotor.Instance.CurrentPos);

    public double SpeedFactor { get; private set; }


    #region private members

    private DeckLayout _deckLayout;

    #endregion


    [UsedImplicitly]
    void Start()
    {
        Instance = this;
//        MessageBox.Initalize(msgBox);
        //       msgBox.SetActive(false);
        _rackPrefabsMap = new Dictionary<string, GameObject>()
        {
            { "1.5mL Conical Tube 4x6", tubeRackLV },
            { "15mL Conical Tube 3x4", tubeRack15 },
            { "50mL Conical Tube 2x3", tubeRack50 },
            { "Chip Caddy", chipCaddy }
        };
#if UNITY_EDITOR || PLATFORM_SUPPORTS_MONO
        var simDashboard = GameObject.Find("Dashboard");
        simDashboard.SetActive(true);
#elif UNITY_WEBGL
        var simDashboard = GameObject.Find("Dashboard");
        simDashboard.SetActive(false);
#endif
    }

    public void SetSimSpeed(double scale)
    {
        SpeedFactor = scale;
    }

    //  private GantryServiceHub _gantryServiceHub;

    public void LoadSetup(ProtocolDto dto)
    {
      
        foreach (var deck in dto.decks)
        {
            var rackGameObject = SetDeck($"Deck-{deck.deckId}", _rackPrefabsMap[deck.rackName]);
            if (deck.rackName.StartsWith("Chip", StringComparison.OrdinalIgnoreCase))
            {
                var chipFactory = rackGameObject.GetComponent<ChipsFactory>();
                chipFactory.CreateChips(deck.deckId, dto.chips, rackGameObject);
                continue;
            }

            var tubeFactory = rackGameObject.GetComponent<TubeFactory>();
            tubeFactory.CreateTubes(deck.deckId, dto.tubes, rackGameObject);
        }


        var stationId = 1;
        foreach (var stationDto in dto.pulseStations)
        {
            var pulseStationGo = GameObject.Find($"Station_{stationId++}");
            var volumeSensors = pulseStationGo.GetComponentsInChildren<VolumeUpdater>();
            var pulseStation = pulseStationGo.GetComponent<PulseStationModel>();
            var leftSensor = volumeSensors.FirstOrDefault(v => v.sensorName == VolumeSensorEnum.Left);
            var rightSensor = volumeSensors.FirstOrDefault(v => v.sensorName == VolumeSensorEnum.Right);
            CreateTubeAdapterStation(pulseStation, stationDto.leftTubeAdapter, leftSensor);
            CreateTubeAdapterStation(pulseStation, stationDto.rightTubeAdapter, rightSensor);
            CreateLVTubeAdapterStation(pulseStation);
        }

     

        foreach (var stationDto in dto.cleaningStations)
        {
            var cleaningStationGo = GameObject.Find($"Station_{stationId++}");
            if (cleaningStationGo == null)
                cleaningStationGo = GameObject.Find($"CleaningStation");
            var cleaningStation = cleaningStationGo.GetComponent<CleaningStationModel>();
            var volumeSensors = cleaningStationGo.GetComponentsInChildren<VolumeUpdater>();
            var leftSensor = volumeSensors.FirstOrDefault(v => v.sensorName == VolumeSensorEnum.Left);
            var rightSensor = volumeSensors.FirstOrDefault(v => v.sensorName == VolumeSensorEnum.Right);
            var tubeAdapter = CreateTubeAdapterStation(cleaningStation, stationDto.leftTubeAdapter, leftSensor);
            PlaceTubeForCleaningStation(tubeAdapter, leftSensor);

            tubeAdapter = CreateTubeAdapterStation(cleaningStation, stationDto.rightTubeAdapter, rightSensor);
            PlaceTubeForCleaningStation(tubeAdapter, rightSensor);
        }
    }

    public void ClearObjects()
    {
        foreach (var go in GameObject.FindGameObjectsWithTag("DragObject"))
        {
            Destroy(go);
        }

        foreach (var go in GameObject.FindGameObjectsWithTag("DropObject"))
        {
            Destroy(go);
        }

        foreach (var go in GameObject.FindGameObjectsWithTag("TubeAdapter"))
        {
            Destroy(go);
        }
    }

    private static GameObject CreateTubeAdapterStation(PulseStationModel pulseStation, string tubeTypeStr,
        Component volumeSensor)
    {
        TubeType tubeType;
        Vector3 position;
        if (tubeTypeStr == "15mL")
        {
            if (volumeSensor == null) return null;
            tubeType = TubeType.Tube_15mL;
            position = new Vector3(0.61f, -0.88f, 0f);
        }
        else
        {
            if (volumeSensor == null) return null;
            tubeType = TubeType.Tube_50mL;
            position = new Vector3(1.08f, -0.88f, 0f);
        }

        return pulseStation.CreateTubeAdapter(tubeType, volumeSensor.transform, position);
    }
    
    private static GameObject CreateLVTubeAdapterStation(PulseStationModel pulseStation)
    {
      

        var position = new Vector3(0.0449f, 11.7f, -0.092f);
        
        return pulseStation.CreateTubeAdapter(TubeType.Tube_1_5mL, pulseStation.transform, position);
    }

    private static void PlaceTubeForCleaningStation(GameObject tubeAdapterGameObject,
        Component volumeSensor)
    {
        var tubeAdapter = tubeAdapterGameObject.GetComponent<TubeAdapter>();
        if (volumeSensor == null) return;
        tubeAdapter.CreateTube(volumeSensor.transform, new Vector3(0, 33.23f, 0));
    }

    private void SaveScene()
    {
        foreach (var deck in _deckLayout.decks)
        {
            var deckName = $"Deck-{deck.deckId}";
            var deckObj = GameObject.Find(deckName);
            deck.rack = FindRack(deckObj);
        }

        Tools.ExportJson(_deckLayout, "DeckLayout.json");
    }

    private static string FindRack(GameObject deckObj)
    {
        for (var i = 0; i < deckObj.transform.childCount; i++)
        {
            var rackObj = deckObj.transform.GetChild(i);
            if (rackObj.name.StartsWith("TubeRack") ||
                rackObj.name.StartsWith("ChipCaddy"))
            {
                return rackObj.name.Replace("(Clone)", "");
            }
        }

        return string.Empty;
    }

    private GameObject SetDeck(string deckName, GameObject rackPrefab)
    {
        var deck = GameObject.Find(deckName);
        var rack = Instantiate(rackPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        rack.transform.SetParent(deck.transform);
        rack.transform.localPosition = new Vector3(0, 0.4f, 0);
        rack.transform.Rotate(0, 180, 0);
        return rack;
    }


    #region Motor Control

    public void MoveXY(double x, double y, double velocity = 0.1)
    {
        var xAxisMotor = XAxisMotor.Instance;
        var yAxisMotor = YAxisMotor.Instance;

        xAxisMotor.MoveAbs(x, velocity);
        yAxisMotor.MoveAbs(y, velocity);
    }

    public void MoveAbs(Axis axis, float position, float velocity)
    {
        switch (axis)
        {
            case Axis.X:
                XAxisMotor.Instance.MoveAbs(position, velocity);
                break;
            case Axis.Y:
                YAxisMotor.Instance.MoveAbs(position, velocity);
                break;
            case Axis.Z:
                ZAxisMotor.Instance.MoveAbs(position, velocity);
                break;
        }
    }

    public void SetMotionAbort(IMotorSim motorSim, IStopInput sensor, bool activated)
    {
        if (activated)
        {
            if (motorSim.AbortInputs.Count == 0)
                motorSim.SetAbortInputs(new List<IStopInput>() { sensor });
            sensor.Activated = true;
            motorSim.MotionAbortEnabled = true;
        }
        else
        {
            motorSim.MotionAbortEnabled = false;
            sensor.Clear();
            sensor.Activated = false;
        }
    }


    public IEnumerator WaitMove(IEnumerable<Axis> axes)
    {
        var motors = new Dictionary<Axis, IMotorSim>()
        {
            { Axis.X, XAxisMotor.Instance },
            { Axis.Y, YAxisMotor.Instance },
            { Axis.Z, ZAxisMotor.Instance }
        };
        foreach (var axis in axes)
        {
            var motor = motors[axis];
            yield return new WaitUntil(() => motor.State is not (MotorState.Started or MotorState.Moving));
        }
    }

    #endregion
}

public enum Axis
{
    X,
    Y,
    Z,
    R
}