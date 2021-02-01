using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace LSB
{
    public class ResponseHandler : MonoBehaviour
    {

        private string REQUEST_TIME_OUT = "Request timeout";
        [Serializable] public class ResultHandler : UnityEvent<ExpressionList> { }
        public ResultHandler OnResult;

        [Serializable] public class ErrorHandler : UnityEvent<string> { }
        public ErrorHandler OnError;

        public void OnResponse(UnityWebRequest request, string word)
        {
            try {  
                ExpressionList expressionList = JsonUtility.FromJson<ExpressionList>(request.downloadHandler.text); 
             
                if (request.responseCode == 200 && OnResult != null)
                    OnResult.Invoke(expressionList);
                if (request.responseCode != 200 && OnError != null)
                { 
                    if(request.error== REQUEST_TIME_OUT)
                    {
                        Toast.Instance.Show("Hubo un problema con la conexión, inténtalo más tarde", 3f, Toast.ToastColor.Red);
                    }
                    else
                    {
                        OnError.Invoke(word);
                    }
                }
            }
            catch (Exception)
            {
                Toast.Instance.Show("Hubo un problema con el servidor, inténtalo más tarde", 3f,Toast.ToastColor.Red);
            }
        }
    }
}
