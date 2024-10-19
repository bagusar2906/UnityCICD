using System.Collections.Generic;
using System.Linq;
using SimulatorClient;
using SimulatorClient.DTOs;
using SimulatorClient.Enums;
using TMPro;
using UnityEngine;

public class DeckFactory : MonoBehaviour
{
    // Start is called before the first frame update


    private void CreateDecks(int row, int[] decks, float pitch)
    {
        var mainDeck = GameObject.Find("Base");
        var originalDeck = GameObject.Find("Deck-7");
        var decFrame = GameObject.Find("DeckFrame");
        var decFrameTransform = decFrame.transform;
        var frame = GameObject.Find("FrameY");
        var localPosition = originalDeck.transform.localPosition;

        var xOffset = row < 2 ? 0 : -0.30f;

        int index = 0;
        foreach (var deckId in decks)
        {
            if (row == 1 && index == 0)
            {
                index++;
                continue; // skip because already have original deck
            }

            var deckClone = Instantiate(originalDeck, mainDeck.transform, true);

            deckClone.name = $"Deck-{deckId}";
            var label = FindChild<TextMeshProUGUI>(deckClone.transform, "Label");
            label.text = deckId.ToString();

            deckClone.transform.localPosition = new Vector3(localPosition.x + xOffset,
                localPosition.y, localPosition.z + index * pitch);

            if (deckId < 6)
            {
                var frameClone = Instantiate(frame, mainDeck.transform, true);
                frameClone.transform.SetParent(decFrameTransform);
                frameClone.transform.localPosition = new Vector3(-0.09f,
                    0, (deckId - 1) * 0.107f - 0.554f);
            }


            index++;
        }
    }

    private static Vector3 CreateStations<T>(string station, IEnumerable<StationConfig> stations,
        float pitch, IDictionary<short, IStationModel> stationModels) where T : IStationModel
    {
        var mainDeck = GameObject.Find("Base");
        GameObject original = null;


        Vector3 lastStationLocation = default;
        var stationConfigs = stations as StationConfig[] ?? stations.ToArray();
        var enumerable = stationConfigs.Select(v => v.id).ToArray();
        var busIds = stationConfigs.Select(v => v.busId).ToArray();
        var index = 0;
        foreach (var stationId in enumerable)
        {
            T stationModel;
            if (original == null)
            {
                original = GameObject.Find(station);
                stationModel = original.GetComponentInChildren<T>();
                original.name = station;
                if (enumerable.Length > 1)
                    original.name = "Station_" + stationId;
                lastStationLocation = original.transform.localPosition;
            }
            else
            {
                var newPulseStation = Instantiate(original, mainDeck.transform, true);
                newPulseStation.name = "Station_" + stationId;
                stationModel = newPulseStation.GetComponentInChildren<T>();
                //positioning pulse station
                var localPosition = original.transform.localPosition;
                newPulseStation.transform.localPosition = new Vector3(localPosition.x,
                    localPosition.y, localPosition.z + (stationId - 1) * pitch);
                lastStationLocation = newPulseStation.transform.localPosition;
            }

            stationModel.Id = stationId;
            stationModel.BusId = busIds[index++];
            stationModels[stationModel.Id] = stationModel;
        }


        return lastStationLocation;
    }

    private static T FindChild<T>(Component parent, string childName)
    {
        foreach (Transform childTransform in parent.transform)
        {
            return childTransform.name == childName
                ? childTransform.GetComponent<T>()
                : FindChild<T>(childTransform, childName);
        }

        return default;
    }

    private void Start()
    {
        var deckPitch = .08475f;
        CreateDecks(1, new[] { 7, 8, 9, 10, 11, 12 }, deckPitch); //1st row
        CreateDecks(2, new[] { 1, 2, 3, 4, 5, 6 }, deckPitch); //2nd row

        /*
        var stationPitch = 0.10325f;
        IDictionary<short, IStationModel> stationModels = new Dictionary<short, IStationModel>();
        var lastPosition =
            CreateStations<PulseStationModel>("PulseStation", new ushort[] { 1, 2, 3, 4 }
                , new short[] { 5, 6, 7, 8 }, stationPitch, stationModels);

        CreateStations<CleaningStationModel>("CleaningStation", new ushort[] { 5 }
            , new short[] { 9 }, deckPitch, stationModels);

        //positioning the object
        var stationModel = GameObject.Find("CleaningStation");
        stationModel.transform.localPosition =
            new Vector3(lastPosition.x, lastPosition.y, lastPosition.z + stationPitch);
            */


        var simulator = GetComponent<Simulator>();
        var setupSelection = simulator.dashboard.GetComponentInChildren<SetupSelection>();

        var gantryModel = GetComponentInChildren<GantryModel>();
        var pulseStation = new PulseServiceHub(gantryModel, setupSelection, simulator)
        {
            OnLoadConfigResponse = OnRequestInfoResponse
        };
        pulseStation.Connect();
    }

    private void OnRequestInfoResponse(RequestInfoDto dto, IDictionary<short, IStationModel> stationModels)
    {
        var stationPitch = 0.10325f;
        var lastPosition = CreateStations<PulseStationModel>("PulseStation",
            dto.stations.Where(v => v.type == StationTypeEnum.Pulse)
            , stationPitch, stationModels);

        var deckPitch = .08475f;
        CreateStations<CleaningStationModel>("CleaningStation",
            dto.stations.Where(v => v.type == StationTypeEnum.Cleaning), deckPitch, stationModels);

        //positioning cleaning station
        int index = 1;
        foreach (var model in stationModels.Values)
        {
            var cleaningStation = model as CleaningStationModel;
            if (cleaningStation == null) continue;
            cleaningStation.transform.localPosition =
                new Vector3(lastPosition.x, lastPosition.y,
                    lastPosition.z + index * stationPitch);
            index++;
        }
;
    }
}