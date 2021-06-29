using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    [CreateAssetMenu()]
    public class TrackDescription : ScriptableObject
    {
        // [SerializeField] RaceTrack track;
        //[SerializeField] GameObject RaceTrack;
        [SerializeField] private string trackName;
        [SerializeField] private string sceneNickname;
        [SerializeField] private Sprite previewImage;
        [SerializeField] private float trackLength;
        public string TrackName => trackName;
        public string SceneNickname => sceneNickname;
        public Sprite PreviewImage => previewImage;

        public float TrackLength
        {
            get
            {
                return trackLength;
            }
            set
            {
                trackLength = value;
            }
        }
        
    }
}
