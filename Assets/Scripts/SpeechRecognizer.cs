using System;
using UnityEngine;
using UnityEngine.UI;
using FantomLib;
using UnityEngine.Events;
using UnityEngine.Android;

namespace LSB
{
    public class SpeechRecognizer : MonoBehaviour
    {
        public Button recongizerButton;
        public Animator circleAnimator;
        public Animator voiceAnimator;
        public SpeechRecognizerController speechRecognizerControl;
        public Text mainText;

        [Serializable] public class ResultHandler : UnityEvent<string> { }
        public ResultHandler OnRequest;

        private void Start()
        {
            RequestUserMicrophonePermission();
        }

        private static void RequestUserMicrophonePermission()
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                Permission.RequestUserPermission(Permission.Microphone);
            }
        }

        public void SwitchWebSearch(string[] words)
        {
            String basicText = words[0].ToLower();
            if (OnRequest != null)
                OnRequest.Invoke(basicText);
        }

        public void OnStartRecognizer()
        {
            
            if (speechRecognizerControl != null)
            {  
                if (speechRecognizerControl.IsSupportedRecognizer && speechRecognizerControl.IsPermissionGranted)
                {
                    if (recongizerButton != null)
                    {
                        recongizerButton.interactable = false;
                    }
                }
                else
                {
                    Toast.Instance.Show("Debe otorgar permisos a la aplicación para usar esta opción", 2.5f);
                    RequestUserMicrophonePermission(); 
                }
            }
        }

        public void OnReady()
        {
            if (circleAnimator != null)
                circleAnimator.SetTrigger("ready");

            if (voiceAnimator != null)
                voiceAnimator.SetTrigger("ready");
        }

        public void OnBegin()
        {
            if (circleAnimator != null)
                circleAnimator.SetTrigger("speech");

            if (voiceAnimator != null)
                voiceAnimator.SetTrigger("speech");
        }

        public void OnResult(string[] words)
        {
            ResetUI();
            SwitchWebSearch(words);
        }

        public void OnError(string message)
        {
            ResetUI();
        }

        public void ResetUI()
        {
            if (circleAnimator != null)
                circleAnimator.SetTrigger("stop");

            if (voiceAnimator != null)
                voiceAnimator.SetTrigger("stop");

            if (recongizerButton != null)
                recongizerButton.interactable = true;
        }

    }
}
