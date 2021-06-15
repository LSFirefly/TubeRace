using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race 
{
    public abstract class RaceTrack : MonoBehaviour
    {
        [Header("Base track properties")]
        [SerializeField] private float radius;
        public float Radius => radius;
        public abstract float GetTrackLength();

        public abstract Vector3 GetPosition(float distance);

        public abstract Vector3 GetDirection(float distance);

        public virtual Quaternion GetRotation(float distance)
        {
            return Quaternion.identity;
        }
    }
}
