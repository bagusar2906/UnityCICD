using System.Collections.Generic;
using SimulatorClient;
using SimulatorClient.DTOs;
using SimulatorClient.Enums;
using UnityEngine;
using Random = System.Random;


public class PulseStationModel : MonoBehaviour, IStationModel
{
    [SerializeField] private GameObject tubeAdapterLV;
    [SerializeField] private GameObject tubeAdapter15;
    [SerializeField] private GameObject tubeAdapter50;
    [SerializeField] private GameObject lowVolumeZ;
    [SerializeField] protected GameObject peristalticPump1;

    private IDictionary<TubeType, GameObject> _tubeAdapters;
    public short BusId { get; set; }
    public short Id { get; set; }
    public virtual IMotorSim Motor1 => lowVolumeZ.GetComponent<NanoZMotor>();
    public virtual IMotorSim Motor0 => peristalticPump1.GetComponentInChildren<PeristalticMotor>();
    public ISliderModel SliderModel => GetComponentInChildren<SliderModel>();
    public IChipClampModel ChipClampModel => GetComponentInChildren<ChipClampModel>();

    public IStopInput LowVolumeLoadCell => GetComponentInChildren<WeightSensor>();

    public VolumeUpdater[] VolumeSensors => GetComponentsInChildren<VolumeUpdater>();
    public WeightSensor[] WeightSensors => GetComponentsInChildren<WeightSensor>();

    private readonly IDictionary<VolumeSensorEnum, VolumeUpdater> _volumeUpdaters =
        new Dictionary<VolumeSensorEnum, VolumeUpdater>();
    
    private readonly IDictionary<VolumeSensorEnum, WeightSensor> _weightSensors =
        new Dictionary<VolumeSensorEnum, WeightSensor>();

    private Random _rnd;

    public virtual void UpdateVolume(VolumeSensorDto dto)
    {
        var weightSensor = _weightSensors[(VolumeSensorEnum)dto.id];
        weightSensor.Weight = dto.weight;
        _volumeUpdaters[(VolumeSensorEnum)dto.id].Volume = weightSensor.Volume;
    }

    public GameObject CreateTubeAdapter(TubeType tubeType, Transform parent, Vector3 localPosition)
    {
       
        var tubeAdapter = Instantiate(_tubeAdapters[tubeType], Vector3.zero, Quaternion.identity);
        var rgb = tubeAdapter.GetComponent<Rigidbody>();
        rgb.mass = (float)(rgb.mass + _rnd.Next(1, 10) * 0.1);
        tubeAdapter.transform.SetParent(parent);
        tubeAdapter.transform.localPosition = localPosition;
        var sensor =  parent.GetComponentInChildren<WeightSensor>();
        sensor.SetInitialWeight(tubeAdapter);
        return tubeAdapter;
    }

     void Start()
     {
         _tubeAdapters = new Dictionary<TubeType, GameObject>()
         {
             { TubeType.Tube_1_5mL, tubeAdapterLV },
             { TubeType.Tube_15mL, tubeAdapter15 },
             { TubeType.Tube_50mL, tubeAdapter50 }
         };
         _rnd = new Random();
        foreach (var volumeSensor in VolumeSensors)
        {
            _volumeUpdaters[volumeSensor.sensorName] = volumeSensor;
        }
        
        foreach (var weightSensor in WeightSensors)
        {
            _weightSensors[weightSensor.sensorName] = weightSensor;
        }
    }
}