using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Race
{
#if UNITY_EDITOR

    [CustomEditor(typeof(RaceTrackRound))]
    public class RaceTrackRoundEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Generate"))
            {
                (target as RaceTrackRound).GenerateTrackData();
            }
        }
    }
#endif
    public class RaceTrackRound : RaceTrack
    {
        [SerializeField] private float circleRadius;
        [SerializeField] private Vector3 circleCenter;
        //[SerializeField] private Vector3 circleNormal;
        [SerializeField] private int division;
        [SerializeField] private Quaternion[] trackSampledRotations;
        [SerializeField] private Vector3[] trackSampledPoints;
        [SerializeField] private float[] trackSampledSegmentLengths;
        [SerializeField] private float trackSampledLength;
        [SerializeField] private bool debugDrawRound;
        [SerializeField] private bool debugDrawSampledPoints;

        private void OnDrawGizmos()
        {
            if (debugDrawRound)
                DrawRound();
            if (debugDrawSampledPoints)
                DrawSampledTrackPoints();
        }

        public void GenerateTrackData()
        {
            Debug.Log("Generate");

            List<Vector3> points = new List<Vector3>();
            List<Quaternion> rotations = new List<Quaternion>();

            for (int i = 0; i< division; i++)
            {
                float angle = 2 * Mathf.PI * i / division;
                Vector3 newPos = new Vector3(Mathf.Cos(angle) * circleRadius, 0, Mathf.Sin(angle) * circleRadius) + circleCenter;
                points.Add(newPos);
            }

            trackSampledPoints = points.ToArray();

            rotations.AddRange(GenerateRotations(trackSampledPoints));
            trackSampledRotations = rotations.ToArray();
           
            {
                trackSampledSegmentLengths = new float[trackSampledPoints.Length - 1];
                trackSampledLength = 0;

                for (int i = 0; i < trackSampledPoints.Length - 1; i++)
                {
                    Vector3 a = trackSampledPoints[i];
                    Vector3 b = trackSampledPoints[i + 1];

                    float segmentLength = (b - a).magnitude;
                    trackSampledSegmentLengths[i] = segmentLength;
                    trackSampledLength += segmentLength;
                }
            }
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        private void DrawSampledTrackPoints()
        {
#if UNITY_EDITOR
            Handles.DrawAAPolyLine(trackSampledPoints);
#endif
        }

        private Quaternion[] GenerateRotations(Vector3[] points)
        {
            List<Quaternion> rotations = new List<Quaternion>();
            float t = 0;

            for (int i = 0; i < points.Length - 1; i++)
            {
                rotations.Add(GenerateRotation(points[i], points[i + 1], t));
                t += 1.0f / (points.Length - 1);
            }

            rotations.Add(GenerateRotation(points[points.Length - 1], points[0], t));
          
            return rotations.ToArray();
        }

        private Quaternion GenerateRotation(Vector3 a, Vector3 b, float t)
        {
            Vector3 dir = (b - a).normalized;
            Vector3 up = Vector3.Lerp(a, b, t);

            Quaternion rotation = Quaternion.LookRotation(dir, up);
            return rotation;
        }

        private void DrawRound()
        {
#if UNITY_EDITOR
            Handles.DrawWireDisc(circleCenter, Vector3.up, circleRadius);
#endif
        }

        public override Vector3 GetDirection(float distance)
        {
            distance = Mathf.Repeat(distance, trackSampledLength);

            for (int i = 0; i < trackSampledSegmentLengths.Length; i++)
            {
                float diff = distance - trackSampledSegmentLengths[i];
                if (diff < 0)
                {
                    return (trackSampledPoints[i + 1] - trackSampledPoints[i]).normalized;
                }
                else
                {
                    distance -= trackSampledSegmentLengths[i];
                }
            }
            return Vector3.forward;
        }

        public override Vector3 GetPosition(float distance)
        {
            distance = Mathf.Repeat(distance, trackSampledLength);

            for (int i = 0; i < trackSampledSegmentLengths.Length; i++)
            {
                float diff = distance - trackSampledSegmentLengths[i];
                if (diff < 0)
                {
                    float t = distance / trackSampledSegmentLengths[i];
                    return Vector3.Lerp(trackSampledPoints[i], trackSampledPoints[i + 1], t);
                }
                else
                {
                    distance -= trackSampledSegmentLengths[i];
                }
            }

            return Vector3.zero;
        }

        public override Quaternion GetRotation(float distance)
        {
            distance = Mathf.Repeat(distance, trackSampledLength);

            for (int i = 0; i < trackSampledSegmentLengths.Length; i++)
            {
                float diff = distance - trackSampledSegmentLengths[i];
                if (diff < 0)
                {
                    float t = distance / trackSampledSegmentLengths[i];
                    return Quaternion.Slerp(trackSampledRotations[i], trackSampledRotations[i + 1], t);
                }
                else
                {
                    distance -= trackSampledSegmentLengths[i];
                }
            }

            return Quaternion.identity;
        }

        public override float GetTrackLength()
        {
            return trackSampledLength;
        }
    }

}

