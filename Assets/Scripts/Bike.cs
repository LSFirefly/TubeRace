using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    public class BikeParameters 
    {
        [Range(0f, 10f)] public float mass;
        [Range(0f, 100f)] public float thrust;
        [Range(0f, 10f)] public float agility;
        public float maxSpeed;

        public bool afterburner;

        public GameObject engineModel, hullModel;
        
    }

    public class Bike: MonoBehaviour
    {
        [SerializeField] private BikeParameters bikeParameters;

       // [SerializeField] private BikeViewController bikeViewController;
    }
}
