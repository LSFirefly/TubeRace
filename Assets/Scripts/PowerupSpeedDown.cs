using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    public class PowerupSpeedDown : Powerup
    {
        [Range(0.0f, 100.0f)]
        [SerializeField] private float speedAmount;
        public override void OnPickedByBike(Bike bike)
        {
            bike.ReduceSpeed(speedAmount);
        }
    }
}
