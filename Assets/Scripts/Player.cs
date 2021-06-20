using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private string nickname;

        public string Nickname => nickname;

        [SerializeField] private Bike activeBike;

        private void Update()
        {
            ControlBike();
        }

        private void ControlBike()
        {
            activeBike.SetForwardThrustAxis(0);
            activeBike.SetHorizontalThrustAxis(0);

            if (!activeBike.IsMovementControlsActive)
                return;

            if(Input.GetKey(KeyCode.W))
            {
                activeBike.SetForwardThrustAxis(1);
            }

            if (Input.GetKey(KeyCode.S))
            {
                activeBike.SetForwardThrustAxis(-1);
            }

            if (Input.GetKey(KeyCode.A))
            {
                activeBike.SetHorizontalThrustAxis(-1);
            }

            if (Input.GetKey(KeyCode.D))
            {
                activeBike.SetHorizontalThrustAxis(1);
            }

            activeBike.EnableAfterburner = Input.GetKey(KeyCode.Space);
           
        }

    }
}
