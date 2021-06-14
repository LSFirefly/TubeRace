using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    public class ObjectPlacer : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int numObjects;
        [SerializeField] private RaceTrack track;

        private void Start()
        {
            float distance = 0;

            for(int i=0; i< numObjects; i++)
            {
                var e = Instantiate(prefab);

                e.transform.position = track.GetPosition(distance);
                e.transform.forward = track.GetDirection(distance);

                distance += track.GetTrackLength() / numObjects;
            }
        }
    }
}