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
        //private static readonly string API_URL = "https://lsbapi.herokuapp.com";
	private static readonly string API_URL = "http://localhost:3000";
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
             
            StartCoroutine(Request(word));
        }

        private IEnumerator Request(string word)
        {
            mainText.color = Color.red;
            mainText.text = "Cargando...";
            UnityWebRequest request = new UnityWebRequest(API_URL, "POST");
            request.timeout = 10;
            byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(createRequest(word)));
             
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
              
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
