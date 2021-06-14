using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Race
{
#if UNITY_EDITOR

    [CustomEditor(typeof(RaceTrackCurved))]
    public class RaceTrackCurvedEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Generate"))
            {
                (target as RaceTrackCurved).GenerateTrackData();
            }
        }
    }
#endif
    public class RaceTrackCurved : RaceTrack
    {

        [SerializeField] private CurvedTrackPoint[] trackPoints;
        [SerializeField] private int division;
        [SerializeField] private Vector3[] trackSampledPoints;
        [SerializeField] private float[] trackSampledSegmentLengths;
        [SerializeField] private float trackSampledLength;
        [SerializeField] private bool debugDrawBezier;
        [SerializeField] private bool debugDrawSampledPoints;

        private void OnDrawGizmos()
        {
            if (debugDrawBezier)
                DrawBezierCurve();
            if (debugDrawSampledPoints)
                DrawSampledTrackPoints();
        }

        public void GenerateTrackData()
        {
            Debug.Log("Generate");

            if (trackPoints.Length < 3)
                return;

            List<Vector3> points = new List<Vector3>();

            for (int i = 0; i < trackPoints.Length - 1; i++)
            {
                points.AddRange(GenerateBezierPoints(trackPoints[i], trackPoints[i + 1], division));
            }

            points.AddRange(GenerateBezierPoints(trackPoints[trackPoints.Length - 1], trackPoints[0], division));

            trackSampledPoints = points.ToArray();

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

            EditorUtility.SetDirty(this);
        }


        private void DrawSampledTrackPoints()
        {
            Handles.DrawAAPolyLine(trackSampledPoints);
        }

        private Vector3[] GenerateBezierPoints(CurvedTrackPoint a, CurvedTrackPoint b, int division)
        {
            return Handles.MakeBezierPoints(
                 a.transform.position,
                 b.transform.position,
                 a.transform.position + a.transform.forward * a.GetLength(),
                 b.transform.position - b.transform.forward * b.GetLength(),
                 division);
        }

        private void DrawBezierCurve()
        {
            if (trackPoints.Length < 3)
                return;

            for (int i = 0; i < trackPoints.Length - 1; i++)
            {
                DrawTrackPartGizmo(trackPoints[i], trackPoints[i + 1]);
            }

            DrawTrackPartGizmo(trackPoints[trackPoints.Length - 1], trackPoints[0]);

        }

        private void DrawTrackPartGizmo(CurvedTrackPoint a, CurvedTrackPoint b)
        {
            Handles.DrawBezier(
               a.transform.position,
               b.transform.position,
               a.transform.position + a.transform.forward * a.GetLength(),
               b.transform.position - b.transform.forward * b.GetLength(),
               Color.green,
               Texture2D.whiteTexture,
               1.0f);
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

            for(int i=0; i< trackSampledSegmentLengths.Length; i++)
            {
                float diff = distance - trackSampledSegmentLengths[i];
                if(diff <0)
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

        public override float GetTrackLength()
        {
            return trackSampledLength;
        }
    }

}