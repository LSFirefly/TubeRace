using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    [CreateAssetMenu()]
    public class TrackDescription : ScriptableObject
    {
        [SerializeField] private string trackName;
        [SerializeField] private string sceneNickname;
        [SerializeField] private Sprite previewImage;
        [SerializeField] private string trackLength;
        public string TrackName => trackName;
        public string SceneNickname => sceneNickname;
        public Sprite PreviewImage => previewImage;
       

    }
}
