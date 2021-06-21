using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    [System.Serializable]
    public class BikeParameters
    {
        public GameObject engineModel;
        public GameObject hullModel;

        [Range(0.0f, 10.0f)] public float mass;
        [Range(0.0f, 100.0f)] public float thrust;
        [Range(0.0f, 100.0f)] public float agility;
        [Range(0.0f, 1.0f)] public float linearDrag;
        [Range(0.0f, 1.0f)] public float rotationDrag;
        [Range(0.0f, 1.0f)] public float collisionBounceFactor;

        public readonly float maxRollAngle = 360.0f;

        public bool afterburner;
        public float maxSpeed;
        public float maxRotationSpeed;
        public float afterburnerThrust;
        public float afterburnerMaxSpeedBonus;
        public float afterburnerHeatGeneration;
        public float afterburnerMaxHeat;
        public float afterburnerCoolSpeed;
    }

    public class Bike : MonoBehaviour
    {
        public static readonly string Tag = "Bike";
        [SerializeField] private BikeParameters bikeParameters;
        [SerializeField] private BikeViewController bikeViewController;
        [SerializeField] private RaceTrack track;
        [SerializeField] private bool isPlayerBike;
        private float forwardThrustAxis;
        private float horizontalThrustAxis;
        private float distance;
        private float velocity;
        private float rotationVelocity;
        private float rollAngle;
        private float afterburnerHeat;
        private float prevDistance;
        private float fuel;
        private float lapTime;
        private float startLapTime;

        private float currentLap = 1;

        public bool IsPlayerBike => isPlayerBike;

        public bool IsMovementControlsActive { get; set; }


        public bool EnableAfterburner { get; set; }


        public float GetDistance()
        {
            return distance;
        }

        public float GetVelocity()
        {
            return velocity;
        }

        public float GetRollAngle()
        {
            return rollAngle;
        }

        public float GetNormalizedHeat()
        {
            if (bikeParameters.afterburnerMaxHeat > 0)
                return afterburnerHeat / bikeParameters.afterburnerMaxHeat;

            return 0;
        }

        public float GetPrevDistance()
        {
            return prevDistance;
        }

        public float GetFuel()
        {
            return fuel;
        }
        public RaceTrack GetTrack()
        {
            return track;
        }

        private void Update()
        {
            UpdateAfterburnerHeat();
            UpdateBikePhysics();
            UpdateBestLapTime();
        }

        private void UpdateAfterburnerHeat()
        {
            afterburnerHeat -= bikeParameters.afterburnerCoolSpeed * Time.deltaTime;

            if (afterburnerHeat < 0)
                afterburnerHeat = 0;

            //Check max heat
            //***

        }

        public void CoolAfterburner()
        {
            afterburnerHeat = 0;
        }

        public float GetNormalizedSpeed()
        {
            return Mathf.Clamp01(velocity / bikeParameters.maxSpeed);
        }

        private void UpdateBikePhysics()
        {
            float dt = Time.deltaTime;

            float FthrustMax = bikeParameters.thrust;
            float Vmax = bikeParameters.maxSpeed;
            float F = forwardThrustAxis * bikeParameters.thrust;

            if (EnableAfterburner && ConsumeFuelForAfterburner(1.0f * Time.deltaTime))
            {
                afterburnerHeat += bikeParameters.afterburnerHeatGeneration * Time.deltaTime;

                F += bikeParameters.afterburnerThrust;
                Vmax += bikeParameters.afterburnerMaxSpeedBonus;
                FthrustMax += bikeParameters.afterburnerThrust;
            }

            F += -velocity * (FthrustMax / Vmax);

            float dv = dt * F;

            velocity += dv;

            if (bikeStatistics.TopSpeed < Mathf.Abs(velocity))
                bikeStatistics.TopSpeed = Mathf.Abs(velocity);

            float ds = velocity * dt;

            if (Physics.Raycast(transform.position, transform.forward, ds))
            {
                velocity = -velocity * bikeParameters.collisionBounceFactor;
                ds = velocity * dt;

                afterburnerHeat += bikeParameters.afterburnerHeatGeneration;
            }

            prevDistance = distance;

            distance += ds;
            if (distance < 0)
                distance = 0;

            rotationVelocity += dt * horizontalThrustAxis * bikeParameters.agility;

            rotationVelocity = Mathf.Clamp(rotationVelocity, -bikeParameters.maxRotationSpeed, bikeParameters.maxRotationSpeed);
            rotationVelocity += -rotationVelocity * bikeParameters.rotationDrag * dt;
            rollAngle += rotationVelocity * dt;


            if (rollAngle < 0)
                rollAngle = 360 + rollAngle;
            if (rollAngle > 360)
                rollAngle = rollAngle - 360;

            SetBikePosition();
        }

        private void SetBikePosition()
        {
            Vector3 bikePos = track.GetPosition(distance);
            Vector3 bikeDir = track.GetDirection(distance);

            Quaternion q = Quaternion.AngleAxis(rollAngle, Vector3.forward);
            Vector3 trackOffset = q * (Vector3.up * track.Radius);

            // transform.position = bikePos - trackOffset;
            // transform.rotation = Quaternion.LookRotation(bikeDir, trackOffset);

            transform.position = bikePos;
            transform.rotation = track.GetRotation(distance);
            transform.Rotate(Vector3.forward, rollAngle, Space.Self);
            transform.Translate(-Vector3.up * track.Radius, Space.Self);

        }

        private void UpdateBestLapTime()
        {
            int lap = (int)(distance / track.GetTrackLength()) + 1; //номер круга
            if (lap > currentLap ) // если увеличился номер круга
            {
                currentLap++;
                lapTime = Time.time - startLapTime;
                startLapTime = Time.time;

                if (lapTime < bikeStatistics.BestLapTime || bikeStatistics.BestLapTime ==0)
                    bikeStatistics.BestLapTime = lapTime;
            }  
        }

        public void SetForwardThrustAxis(float val)
        {
            forwardThrustAxis = val;
        }

        public void SetHorizontalThrustAxis(float val)
        {
            horizontalThrustAxis = val;
        }

        public bool ConsumeFuelForAfterburner(float amount)
        {
            if (fuel <= amount)
                return false;

            fuel -= amount;

            return true;
        }

        public void AddFuel(float amount)
        {
            fuel += amount;
            fuel = Mathf.Clamp(fuel, 0.0f, 100.0f);
        }

        public void ReduceSpeed(float amount)
        {
            velocity -= amount;
            if (velocity < 0)
                velocity = 0;
        }

        public class BikeStatistics
        {
            public float TopSpeed;
            public float TotalTime;
            public float BestLapTime;
            public int RacePlace;
        }

        private BikeStatistics bikeStatistics;
        public BikeStatistics Statistics => bikeStatistics;

        private void Awake()
        {
            bikeStatistics = new BikeStatistics();
        }

        private float raceStartTime;

        public void OnRaceStart()
        {
            raceStartTime = Time.time;
            startLapTime = raceStartTime;
        }

        public void OnRaceEnd()
        {
            bikeStatistics.TotalTime = Time.time - raceStartTime;
        }

    }
}
