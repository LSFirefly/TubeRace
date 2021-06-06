using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private RaceTrack track;
        [SerializeField] private float rollAngle;
        [SerializeField] private float distance;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float maxRollAngle = 360.0f;
        [SerializeField] [Range(0.0f, 20.0f)] private float radiusModifier=1;

        private void Update()
        {
            UpdateObstacleRotation();
        }

        private void OnValidate()
        {
            SetObstaclePosition();
        }

        private void SetObstaclePosition()
        {
            Vector3 obstaclePos = track.GetPosition(distance);
            Vector3 obstacleDir = track.GetDirection(distance);

            Quaternion q = Quaternion.AngleAxis(rollAngle, Vector3.forward);
            Vector3 trackOffset = q * (Vector3.up * radiusModifier * track.Radius);

            transform.position = obstaclePos - trackOffset;
            transform.rotation = Quaternion.LookRotation(obstacleDir, trackOffset);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 centerLinePos = track.GetPosition(distance);
            Gizmos.DrawSphere(centerLinePos, track.Radius);
        }

        private void UpdateObstacleRotation()
        {
            float dt = Time.deltaTime;
            rollAngle += dt * rotationSpeed;

            rollAngle = rollAngle % maxRollAngle;

            SetObstaclePosition();
        }
    }
}
