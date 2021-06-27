using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    public class MainMenuViewController : MonoBehaviour
    {
        [SerializeField] private TrackSelectionViewController trackSelectionViewController;
        [SerializeField] private OptionsViewController optionsViewController;



        public void OnButtonNewGame()
        {
            trackSelectionViewController.gameObject.SetActive(true);

            gameObject.SetActive(false);
        }

        public void OnButtonOptions()
        {
            optionsViewController.gameObject.SetActive(true);

            gameObject.SetActive(false);
        }

        public void OnButtonExit()
        {
            Application.Quit();
        }

    }
}
