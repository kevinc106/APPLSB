using System;
using System.Collections;
using System.Collections.Generic;
using System.IO; 
using UnityEngine;

namespace LSB
{
    public class AnimatorControllerStates : MonoBehaviour
    {
        private List<string> animationList = new List<string>();
        void Start()
        {
            /*AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>("Assets/Default.controller");
            var rootStateMachine = controller.layers[0].stateMachine;
            List<ChildAnimatorState> animatorStates = new List<ChildAnimatorState>(rootStateMachine.states);
            List<string> stateCodes = new List<string>();
            foreach (var animatorState in animatorStates)
            { 
                animationList.Add(animatorState.state.name);
            } */
            TextAsset codes = (TextAsset) Resources.Load("Codes");
            
            char[] delimiters = new char[] { '\r', '\n' };
            foreach(string s in codes.text.Split(delimiters,StringSplitOptions.RemoveEmptyEntries))
            {
                animationList.Add(s);
            } 
            /*animationList.AddRange(new string[] {
                // ALFABETO DACTILOLOGICO
                "#00101", "#00102", "#00103", "#00105", "#00106", "#00107", "#00108", "#00109", "#00110",
                "#00111", "#00112", "#00113", "#00115", "#00116", "#00117", "#00118", "#00119", "#00120",
                "#00121", "#00123", "#00124", "#00125", "#00126", "#00127", "#00128", "#00129", "#00130",
                // NUMEROS
                "#32101", "#32102", "#32103", "#32104", "#32105", "#32106", "#32107", "#32108", "#32109", "#32110",
                // SALUDOS
                "#43101", "#43102", "#43103", "#43104", "#43105", "#43106", "#43107",
                // LUGARES
                "#28101", "#28110", "#28113",
                // PRONOMBRES
                "#39101", "#39102", "#39104",
                // VERBOS
                "#47101","#47102","#47103","#47104","#47105","#47106","#47107","#47108", "#47118",
                // OPUESTOS
                "#33113",
		        //TIEMPOS
		        "#99103","#99101","#99102"
            });*/
        }

        public bool hasStateName(string stateName)
        {
            return animationList.Contains(stateName);
        }

        public bool hasAllStateNames(List<string> stateNames)
        {
            foreach(string stateName in stateNames)
            {
                if(!hasStateName(stateName))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
