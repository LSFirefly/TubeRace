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

        public bool afterburner;
        public float maxSpeed;
        public float afterburnerThrust;
        public float afterburnerMaxSpeedBonus;
        public float afterburnerHeatGeneration;
        public float afterburnerMaxHeat;
        public float afterburnerCoolSpeed;
    }

    public class Bike : MonoBehaviour
    {
        public static readonly string tag = "Bike";
        [SerializeField] private BikeParameters bikeParameters;
        [SerializeField] private BikeViewController bikeViewController;
        [SerializeField] private RaceTrack track;
        private float forwardThrustAxis;
        private float horizontalThrustAxis;
        private float distance;
        private float velocity;
        private float rollAngle;
        private float afterburnerHeat;
        private float prevDistance;
        private float fuel;


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
            rollAngle += dt * horizontalThrustAxis * bikeParameters.agility;

            float ds = velocity * dt;

            if (Physics.Raycast(transform.position, transform.forward, ds))
            {
                velocity = -velocity * bikeParameters.collisionBounceFactor;
                ds = velocity * dt;
            }

            prevDistance = distance;

            distance += ds;
            if (distance < 0)
                distance = 0;
       
            rollAngle += -rollAngle * bikeParameters.rotationDrag * dt;

            SetBikePosition();

        }

        private void SetBikePosition()
        {
            Vector3 bikePos = track.GetPosition(distance);
            Vector3 bikeDir = track.GetDirection(distance);

            Quaternion q = Quaternion.AngleAxis(rollAngle, Vector3.forward);
            Vector3 trackOffset = q * (Vector3.up * track.Radius);

            transform.position = bikePos - trackOffset;
            transform.rotation = Quaternion.LookRotation(bikeDir, trackOffset);
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

    }
}
