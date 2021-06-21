using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Race
{
    public class RaceController : MonoBehaviour
    {
        public enum RaceMode
        {
            Laps,
            Time,
            LastStanding
        }

        [SerializeField] private int maxLaps;
        [SerializeField] private RaceMode raceMode;
        [SerializeField] private UnityEvent eventRaceStart;
        [SerializeField] private UnityEvent eventRaceFinished;
        [SerializeField] private Bike[] bikes;
        [SerializeField] private int countdownTimer;
        [SerializeField] private RaceCondition[] conditions;
        [SerializeField] private RaceTrack track;
        [SerializeField] private RaceResultsViewController raceResultsViewController;
        private float countTimer;
        private List<Bike> activeBikes;
        private List<Bike> finishedBikes;

        public int MaxLaps => maxLaps;
        public Bike[] Bikes => bikes;
        public int CountdownTimer => countdownTimer;
        public float CountTimer => countTimer;
        public bool IsRaceActive { get; private set; }

        private void Start()
        {
            StartRace();
        }

        private void Update()
        {
            if (!IsRaceActive)
                return;

            UpdateBikeRacePositions();
            UpdateRacePrestart();
            UpdateConditions();
        }

        public  void StartRace()
        {
            activeBikes = new List<Bike>(bikes);
            finishedBikes = new List<Bike>();

            IsRaceActive = true;

            countTimer = countdownTimer;

            foreach(RaceCondition condition in conditions)
                condition.OnRaceStart();

            foreach (Bike bike in bikes)
                bike.OnRaceStart();

            eventRaceStart?.Invoke();
            
        }

        public void EndRace()
        {
            IsRaceActive = false;

            foreach (RaceCondition condition in conditions)
            {
                condition.OnRaceEnd();
            }

            eventRaceFinished?.Invoke();
        }

        private void UpdateRacePrestart()
        {
            if (countTimer > -1)
            {
                countTimer -= Time.deltaTime;

                if (countTimer < 0)
                {
                    foreach (Bike bike in bikes)
                        bike.IsMovementControlsActive = true;
                }
            }
        }

        private void UpdateConditions()
        {
            if (IsRaceActive)
                return;

            foreach(RaceCondition condition in conditions)
            {
                if (!condition.IsTriggered)
                    return;

            }

            EndRace();
        }

        private void UpdateBikeRacePositions()
        {
            //if(activeBikes.Count == 0)
            //{
            //    EndRace();
            //    return;
            //}

            foreach(Bike bike in activeBikes)
            {
                if (finishedBikes.Contains(bike))
                    continue;

                float dist = bike.GetDistance();
                float totalRaceDistance = maxLaps * track.GetTrackLength();

                if(dist > totalRaceDistance)
                {
                    finishedBikes.Add(bike);
                    bike.Statistics.RacePlace = finishedBikes.Count;
                    bike.OnRaceEnd();

                    if(bike.IsPlayerBike)
                    {
                        raceResultsViewController.Show(bike.Statistics);
                    }
                }
            }
        }
    }
}
