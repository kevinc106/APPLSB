using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSB
{
    public class SpeedController : MonoBehaviour
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
                commander.setSlowSpeed();
            }
            closePanel();
        }

        public void setMediumSpeed()
        {
            if (commander != null)
            {
                commander.setMediumSpeed();
            }
            closePanel();
        }

        public void setFastSpeed()
        {
            if (commander != null)
            {
                commander.setFastSpeed();
            }
            closePanel();
        }
    }
}
