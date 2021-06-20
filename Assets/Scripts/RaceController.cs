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
        private float countTimer;

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

            UpdateRacePrestart();
            UpdateConditions();
        }

        public  void StartRace()
        {
            IsRaceActive = true;

            countTimer = countdownTimer;

            foreach(RaceCondition condition in conditions)
            {
                condition.OnRaceStart();
            }

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
            if (countTimer > 0)
            {
                countTimer -= Time.deltaTime;

                if (countTimer <= 0)
                {
                    foreach (Bike bike in bikes)
                        bike.IsMovementControlsActive = true;
                }
            }
        }

        private void UpdateConditions()
        {
            if (!IsRaceActive)
                return;

            foreach(RaceCondition condition in conditions)
            {
                if (!condition.IsTriggered)
                    return;

            }

            EndRace();
        }
    }
}
