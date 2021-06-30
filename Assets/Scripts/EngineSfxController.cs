using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    public class EngineSfxController : MonoBehaviour
    {
        [SerializeField] private AudioSource engineSource;
        [SerializeField] private Bike bike;
        [Range(0.0f, 1.0f)] [SerializeField] private float velocityPitchModifier;

        private void Update()
        {
            UpdateEngineSoundSimple();
        }

        private void UpdateEngineSoundSimple()
        {
            engineSource.pitch = 1.0f + velocityPitchModifier* bike.GetNormalizedSpeed();
        }
    }
}
