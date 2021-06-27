using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    public class PauseViewController : MonoBehaviour
    {
        public static readonly string MainMenuScene = "MainMenuScene";
        [SerializeField] private RectTransform content;
        [SerializeField] private RaceController raceController;

        private void Start()
        {
            content.gameObject.SetActive(false);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if (raceController.IsRaceActive)
                {
                    content.gameObject.SetActive(!content.gameObject.activeInHierarchy);

                    UpdateGameActivity(!content.gameObject.activeInHierarchy);
                }
            }

           
        }

        private void UpdateGameActivity(bool flag)
        {
            if(flag)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
        }

        public void OnButtonContinue()
        {
            UpdateGameActivity(true);
            content.gameObject.SetActive(false);
        }

        public void OnButtonEndRace()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(MainMenuScene);
        }

    }
}
