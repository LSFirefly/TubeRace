using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Race
{
#if UNITY_EDITOR
    [CustomEditor(typeof(SplineMeshProxy))]
    public class SplineMeshProxyEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Update"))
            {
                (target as SplineMeshProxy).UpdatePoints();
            }
        }
    }
#endif

    [RequireComponent(typeof(SplineMesh.Spline))]
    public class SplineMeshProxy : MonoBehaviour
    {
        [SerializeField] private RaceTrackCurved curvedTrack;
        //[SerializeField] private CurvedTrackPoint pointA;
        //[SerializeField] private CurvedTrackPoint pointB;


        public void UpdatePoints()
        {
            var spline = GetComponent<SplineMesh.Spline>();

            spline.nodes.Clear();

            CurvedTrackPoint[] trackPoints = curvedTrack.TrackPoints;

            for (int i = 0; i < trackPoints.Length; i++)
            {
                SplineMesh.SplineNode node = new SplineMesh.SplineNode(trackPoints[i].transform.position,
                    trackPoints[i].transform.position + trackPoints[i].transform.forward * trackPoints[i].GetLength());
                spline.nodes.Add(node);
            }

            spline.nodes.Add(spline.nodes[0]);

            //var n0 = spline.nodes[0];
            //n0.Position = pointA.transform.position;
            //n0.Direction = pointA.transform.position + pointA.transform.forward * pointA.GetLength();

            //var n1 = spline.nodes[1];
            //n1.Position = pointB.transform.position;
            //n1.Direction = pointB.transform.position + pointB.transform.forward * pointB.GetLength();

        }
    }
}