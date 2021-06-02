using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    public class RaceTrackLinear : RaceTrack
    {
        [Header("Linear track properties")]
        [SerializeField] private Transform start;
        [SerializeField] private Transform end;
        public override Vector3 GetDirection(float distance)
        {
            distance = Mathf.Clamp(distance, 0, GetTrackLength());
            return (end.position - start.position).normalized;
        }

        public override Vector3 GetPosition(float distance)
        {
            distance = Mathf.Clamp(distance, 0, GetTrackLength());

            Vector3 direction = end.position - start.position;
            direction = direction.normalized;

            return start.position + direction * distance;
        }

        public override float GetTrackLength()
        {
            Vector3 direction = end.position - start.position;
            return direction.magnitude;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(start.position, end.position);
        }

        #region Test

        [SerializeField] private float testDistance;
        [SerializeField] private Transform testObject;
        [SerializeField] [Range(-10, 10)] private float speed;

        private void OnValidate()
        {
            testObject.position = GetPosition(testDistance);
            testObject.forward = GetDirection(testDistance);
        }

        public void Update()
        {
            MoveTestObject();
        }

        private void MoveTestObject()
        {
            testDistance += speed * Time.deltaTime;

            if (testDistance > GetTrackLength())
                testDistance = 0;
            else if (testDistance < 0)
                testDistance = GetTrackLength();

            testObject.position = GetPosition(testDistance);
            testObject.forward = GetDirection(testDistance);
        }


        #endregion
    }
}
