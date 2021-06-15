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

        [SerializeField] private bool randomizeRotation;
        [SerializeField] private bool randomizePosition;
        [SerializeField] private int seed;

        private void Start()
        {
            float position = 0;
            Random.InitState(seed);

            for (int i = 0; i < numObjects; i++)
            {
                if (randomizePosition)
                {
                    position = Random.value * track.GetTrackLength();
                    InstantiateObject(position);
                }
                else
                {
                    InstantiateObject(position);
                    position += track.GetTrackLength() / numObjects;
                }
            }

        }

        private void InstantiateObject(float position)
        {
            var e = Instantiate(prefab);

            e.transform.position = track.GetPosition(position);
            e.transform.rotation = track.GetRotation(position);

            if (randomizeRotation)
            {
                e.transform.Rotate(Vector3.forward, UnityEngine.Random.Range(0, 360), Space.Self);
            }
        }
    }
}