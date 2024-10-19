using System;
using System.Collections.Generic;

namespace MultiSamplesPulse
{
    
    [Serializable]
    public class DeckLayout
    {
        public DeckLayout()
        {
            decks = new List<Deck>();
            for (var i = 1; i < 13; i++)
            {
                decks.Add(new Deck()
                {
                    deckId = i,
                    rack = ""
                });
            }

            decks[0].rack = "TubeRack-50ml";
            decks[2].rack = "ChipCaddy";
            decks[6].rack = "TubeRack-1.5ml";
            decks[9].rack = "TubeRack-15ml";
            decks[11].rack = "ChipCaddy";
            

            pulseStations = new List<PulseStationConfig>()
            {
                new()
                { 
                    id = 1,
                    leftTubeAdapter = "50mL",
                    nanoTubeAdapter = "1.5mL",
                    rightTubeAdapter = "15mL"
                },
                new()
                { 
                    id = 2,
                    leftTubeAdapter = "50mL",
                    nanoTubeAdapter = "None",
                    rightTubeAdapter = "50mL"
                },
                new()
                { 
                    id = 3,
                    leftTubeAdapter = "15mL",
                    nanoTubeAdapter = "None",
                    rightTubeAdapter = "50mL"
                },
                new()
                { 
                    id = 4,
                    leftTubeAdapter = "15mL",
                    nanoTubeAdapter = "None",
                    rightTubeAdapter = "50mL"
                }
            };
        }
        
        public List<Deck> decks;

        public List<PulseStationConfig> pulseStations;
    }
}