using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race 
{
    public abstract class Powerup : MonoBehaviour
    {
        [SerializeField] private RaceTrack track;
        [SerializeField] private float rollAngle;
        [SerializeField] private float distance;

        private void Update()
        {
            UpdateBikes();
        }

        private void UpdateBikes()
        {
        foreach (GameObject bikeGameObject in GameObject.FindGameObjectsWithTag(Bike.tag))
            {
                Bike bike = bikeGameObject.GetComponent<Bike>();
                float prevDistance = bike.GetPrevDistance();
                float currDistance = bike.GetDistance();

                if(prevDistance < distance && currDistance > distance)
                {
                    //limit angles
                    OnPickedByBike(bike);
                }
            }
        }

        public abstract void OnPickedByBike(Bike bike);


        private void OnValidate()
        {
            SetPowerupPosition();
        }

        private void SetPowerupPosition()
        {
            Vector3 powerupPos = track.GetPosition(distance);
            Vector3 powerupDir = track.GetDirection(distance);

            Quaternion q = Quaternion.AngleAxis(rollAngle, Vector3.forward);
            Vector3 trackOffset = q * (Vector3.up * 0);

            transform.position = powerupPos - trackOffset;
            transform.rotation = Quaternion.LookRotation(powerupDir, trackOffset);
        }
    }
}
