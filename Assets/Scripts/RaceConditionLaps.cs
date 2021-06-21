using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    public class RaceConditionLaps : RaceCondition
    {
        [SerializeField] private RaceController raceController;

        private void Update()
        {
            if (!raceController.IsRaceActive && IsTriggered)
                return;

            Bike[] bikes = raceController.Bikes;

            foreach(Bike bike in bikes)
            {
                int laps = (int)(bike.GetDistance() / bike.GetTrack().GetTrackLength());
                if (laps < raceController.MaxLaps)
                    return;
            }

            IsTriggered = true;
        }
    }
}
