using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Bike targetBike;
        [SerializeField] private float minFov = 60;
        [SerializeField] private float maxFov =85;
        [SerializeField] private float shakeFactor;
        [SerializeField] private AnimationCurve shakeCurve;

        private Vector3 initialLocalPosition;
        private void Start()
        {
            initialLocalPosition = Camera.main.transform.localPosition;
        }

        private void Update()
        {
            UpdateFov();
            UpdateShake();
        }

        private void UpdateFov()
        {
            var cam = Camera.main;
            var t = targetBike.GetNormalizedSpeed();

            cam.fieldOfView = Mathf.Lerp(minFov, maxFov, t);
        }

        private void UpdateShake()
        {
            if (Time.timeScale > 0)
            {
                var cam = Camera.main;
                var t = targetBike.GetNormalizedSpeed();
                var curveValue = shakeCurve.Evaluate(t);

                var randomVector = UnityEngine.Random.insideUnitSphere * shakeFactor;
                randomVector.z = 0;

                cam.transform.localPosition = initialLocalPosition + randomVector * curveValue;
            }
        }
    }
}
