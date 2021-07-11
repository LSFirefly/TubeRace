using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    public class ComplexEngineSfxController : MonoBehaviour
    {
        [SerializeField] private Bike bike;

        [SerializeField] private AudioSource sfxLow;
        [SerializeField] private AudioSource sfxHigh;
        [SerializeField] private AudioSource sfxLoud;

        [SerializeField] private AnimationCurve curveLow;
        [SerializeField] private AnimationCurve curveHigh;
        [SerializeField] private AnimationCurve curveLoud;

        [SerializeField] private AudioSource sfxSonicBoom;

        public const float PitchFactor = 2f;
        [SerializeField] private float superSonicSpeed;
        [SerializeField] private AnimationCurve curveSonic;

        public bool IsSuperSonic { get; private set; }

        private void Update()
        {
            SetSuperSonic(Mathf.Abs(bike.GetVelocity()) > superSonicSpeed);

            if(sfxSonicBoom.isPlaying)
            {
                var t = Mathf.Clamp01(sfxSonicBoom.time / sfxSonicBoom.clip.length);
                sfxSonicBoom.volume = curveSonic.Evaluate(t);
            }

            UpdateDynamicEngineSound();
        }

        private void UpdateDynamicEngineSound()
        {
            if(IsSuperSonic)
            {
                sfxLow.volume = 0;
                sfxHigh.volume = 0;
                sfxLoud.volume = 0;
                return;
            }

            // var t = bike.GetNormalizedSpeed();
            var t = Mathf.Clamp01(bike.GetVelocity() / superSonicSpeed);

            sfxLow.volume = curveLow.Evaluate(t);

            sfxLow.pitch = 1.0f + PitchFactor * t;

            sfxHigh.volume = curveHigh.Evaluate(t);

            sfxHigh.pitch = 1.0f + PitchFactor * t;

            sfxLoud.volume = curveLoud.Evaluate(t);
        }

        public void SetSuperSonic(bool flag)
        {
            if(!IsSuperSonic && flag)
            {
                sfxSonicBoom.Play();
            }
            IsSuperSonic = flag;
        }
    }
}