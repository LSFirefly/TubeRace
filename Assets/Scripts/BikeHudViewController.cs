using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Race.UI
{
    public class BikeHudViewController : MonoBehaviour
    {
        [SerializeField] private Text labelSpeed;
        [SerializeField] private Text labelDistance;
        [SerializeField] private Text labelRollAngle;
        [SerializeField] private Text labelLapNumber;

        [SerializeField] private Bike bike;

        private void Update()
        {
            int velocity = (int)bike.GetVelocity();
            int distance = (int)bike.GetDistance();
            int roll = (int)bike.GetRollAngle();
            int laps = (int)(bike.GetDistance() / bike.GetTrack().GetTrackLength());
            labelSpeed.text = "Speed: " + velocity.ToString() + " m/s";
            labelDistance.text = "Distance: " + distance.ToString() + " m";
            labelRollAngle.text = "Angle: " + roll.ToString() + " deg";
            labelLapNumber.text = "Lap: " + (laps + 1).ToString();
           

        }
    }
}
