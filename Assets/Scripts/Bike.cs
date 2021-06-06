using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    [System.Serializable]
    public class BikeParameters 
    {
        [Range(0.0f, 10.0f)] public float mass;
        [Range(0.0f, 100.0f)] public float thrust;
        [Range(0.0f, 100.0f)] public float agility;
        [Range(0.0f, 1.0f)] public float linearDrag;
        [Range(0.0f, 1.0f)] public float rotationDrag;
        [Range(0.0f, 1.0f)] public float collisionBounceFactor;
        public float maxSpeed;
        public bool afterburner;
        public GameObject engineModel, hullModel;
    }

    public class Bike: MonoBehaviour
    {
        [SerializeField] private BikeParameters bikeParameters;
        [SerializeField] private BikeViewController bikeViewController;
        [SerializeField] private RaceTrack track;
        private float forwardThrustAxis;
        private float horizontalThrustAxis;
        private float distance;
        private float velocity;
        private float rollAngle;

        private void Update()
        {
            UpdateBikePhysics();
        }

        private void UpdateBikePhysics()
        {
            float dt = Time.deltaTime;
                       
            velocity += dt * forwardThrustAxis * bikeParameters.thrust;
            rollAngle += dt * horizontalThrustAxis * bikeParameters.agility;

            velocity = Mathf.Clamp(velocity, -bikeParameters.maxSpeed, bikeParameters.maxSpeed);

            float ds = velocity * dt;

            if (Physics.Raycast(transform.position, transform.forward, ds))
            {
                velocity = -velocity*bikeParameters.collisionBounceFactor;
                ds = velocity * dt;   
            }

            distance += ds;
            if (distance < 0)
                distance = 0;

            velocity += -velocity * bikeParameters.linearDrag*dt;         
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

        private void MoveBike()
        {
            float currentForwardVelocity = forwardThrustAxis* bikeParameters.maxSpeed;
            Vector3 forwardMoveDelta = transform.forward * currentForwardVelocity * Time.deltaTime;
            transform.position += forwardMoveDelta; 
        }

        public void SetForwardThrustAxis(float val)
        {
            forwardThrustAxis = val;
        }

        public void SetHorizontalThrustAxis(float val)
        {
            horizontalThrustAxis = val;
        }

    }
}
