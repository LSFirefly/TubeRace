using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Race
{
    public class RaceResultsViewController : MonoBehaviour
    {
        [SerializeField] private Text place;
        [SerializeField] private Text topSpeed;
        [SerializeField] private Text totalTime;
        [SerializeField] private Text bestLapTime;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Show(Bike.BikeStatistics stats)
        {
            gameObject.SetActive(true);

            place.text = "Place: " + stats.RacePlace.ToString();
            topSpeed.text = "Top speed: " + ((int)(stats.TopSpeed)).ToString() + " m/s";
            totalTime.text = "Total time: " +  stats.TotalTime.ToString() + " seconds";
            bestLapTime.text = "Best lap time: " + stats.BestLapTime.ToString() + " seconds";

        }
    }
}
