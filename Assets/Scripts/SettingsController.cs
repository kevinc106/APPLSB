using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSB
{
    public class SettingsController : MonoBehaviour
    {
        public GameObject panel;
        public AnimatorCommander commander; 
        public void openPanel()
        {
            if (panel != null)
            {
                panel.SetActive(true); 
            }
        }

        public void closePanel()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        public void setSlowSpeed()
        {
            if(commander != null)
            {
                commander.SetSlowSpeed();
            }
            closePanel();
        }

        public void setMediumSpeed()
        {
            if (commander != null)
            {
                commander.SetMediumSpeed();
            }
            closePanel();
        }

        public void setFastSpeed()
        {
            if (commander != null)
            {
                commander.SetFastSpeed();
            }
            closePanel();
        }

        private void Update()
        {
            goBack(); 
        }

        private void goBack()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (panel.activeSelf)
                {
                    closePanel();
                }
                else
                {
                    Application.Quit();
                }
            }
        } 
    }
}
