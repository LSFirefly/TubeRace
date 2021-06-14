using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    public class CurvedTrackPoint : MonoBehaviour
    {
        [SerializeField] private float length = 1.0f;

        public float GetLength()
        {
            return length;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(transform.position, 10.0f);
        }
    }
}
