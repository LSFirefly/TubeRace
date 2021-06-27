using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Race
{
    public class TrackEntryViewController : MonoBehaviour
    {
        [SerializeField] private TrackDescription trackDescription;
        [SerializeField] private Text trackName;
        [SerializeField] private Image preview;
        [SerializeField] private Text trackLength;

        private TrackDescription activeDescription;

        private void Start()
        {
            if (trackDescription != null)
                SetViewValues(trackDescription);
        }

        public void SetViewValues(TrackDescription desc)
        {
            activeDescription = desc;
            trackName.text = desc.TrackName;
            preview.sprite = desc.PreviewImage;
        }
        
        public void OnButtonStartLevel()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(activeDescription.SceneNickname);
        }
    }
}
