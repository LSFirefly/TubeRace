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
        [SerializeField] private Text labelHeat;
        [SerializeField] private Text labelFuel;

        [SerializeField] private Bike bike;

        private void Update()
        {
            int velocity = (int)bike.GetVelocity();
            int distance = (int)bike.GetDistance();
            int roll = (int)bike.GetRollAngle();
            int laps = (int)(bike.GetDistance() / bike.GetTrack().GetTrackLength());
            int heat = (int)(bike.GetNormalizedHeat() * 100.0f);
            int fuel = (int)bike.GetFuel();
            labelSpeed.text = "Speed: " + velocity + " m/s";
            labelDistance.text = "Distance: " + distance + " m";
            labelRollAngle.text = "Angle: " + roll + " deg";
            labelLapNumber.text = "Lap: " + (laps + 1);
            labelHeat.text = "Heat: " + heat;
            labelFuel.text = "Fuel: " + fuel;
        }
    }
}
