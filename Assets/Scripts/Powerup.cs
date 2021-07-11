using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Race
{
    public abstract class Powerup : MonoBehaviour
    {
        [SerializeField] private RaceTrack track;
        [SerializeField] [Range(0.0f, 360.0f)] private float rollAngle;
        [SerializeField] private float distance;
        [SerializeField] private float offsetAngle = 10.0f;
        [SerializeField] private AudioSource audioSource;
        //[SerializeField] private float radiusModifier = 1;


        private void Update()
        {
            UpdateBikes();
        }

        private void UpdateBikes()
        {
            foreach (GameObject bikeGameObject in GameObject.FindGameObjectsWithTag(Bike.Tag))
            {
                Bike bike = bikeGameObject.GetComponent<Bike>();
                float prevDistance = bike.GetPrevDistance();
                float currDistance = bike.GetDistance();

                if (prevDistance < distance && currDistance > distance)
                {
                    //limit angles
                    float bikeRollAngle = bike.GetRollAngle();
                    
                    float leftBorder = rollAngle - offsetAngle;
                    float rightBorder = rollAngle + offsetAngle;

                    if (bikeRollAngle > leftBorder && bikeRollAngle < rightBorder)
                    {
                        OnPickedByBike(bike);
                        audioSource.volume = 10;
                        audioSource.Play();
                    }
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
            //    Vector3 trackOffset = q * (Vector3.up * 0);

            Vector3 trackOffset = q * (Vector3.up * track.Radius);

            transform.position = powerupPos - trackOffset;
            transform.rotation = Quaternion.LookRotation(powerupDir, trackOffset);
        }
    }
}
