using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Race
{
    public class OptionsViewController : MonoBehaviour
    {
        [SerializeField] private Dropdown screenResolution;
        [SerializeField] private List<string> screenResolutionsList = new List<string> { "640x480", "800x600", "1920x1080"};
      

        private void Awake()
        {
            gameObject.SetActive(false);
            screenResolution.AddOptions(screenResolutionsList);
            SetScreenResolution();
        }

        public void OnButtonApply()
        {
            SetScreenResolution();

            UnityEngine.SceneManagement.SceneManager.LoadScene(PauseViewController.MainMenuScene);
        }

        private void SetScreenResolution()
        {
            string value = screenResolution.captionText.text;
            string[] sizes = value.Split('x');
            int width = System.Convert.ToInt32(sizes[0]);
            int height = System.Convert.ToInt32(sizes[0]);
            Screen.SetResolution(width, height, false);
        }
    }
}
