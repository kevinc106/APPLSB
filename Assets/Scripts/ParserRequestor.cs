using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace LSB
{
    public class ParserRequestor : MonoBehaviour
    {
        private static readonly string API_URL = "https://lsbapi.herokuapp.com";

        [Serializable] public class ResultHandler : UnityEvent<UnityWebRequest, string> { }
        public ResultHandler OnResult;

        public InputField inputField;

        public Text mainText;

        private void Start()
        {
            //OnRequest("venir");
        }

        public void OnRequest(string word)
        {
            inputField.text = "";
            Debug.Log("REQUEST: " + word);
            StartCoroutine(Request(word));
        }

        private IEnumerator Request(string word)
        {
            mainText.color = Color.red;
            mainText.text = "Cargando...";
            UnityWebRequest request = new UnityWebRequest(API_URL, "POST");
            request.timeout = 10;
            byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(createRequest(word)));
            Debug.Log("JSON?1 - " + JsonUtility.ToJson(createRequest(word)));
           // Debug.Log("BODYRAW?: " + bodyRaw.ToString());
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            Debug.Log("uploadHandler?: " + request.uploadHandler.contentType); 
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            if (OnResult != null)
            {
                mainText.color = Color.black;
                OnResult.Invoke(request, word);
            }
        }

        private LSBRequest createRequest(string word)
        {
            LSBRequest request = new LSBRequest();
            request.word = word;
            return request;
        }
    }
}
