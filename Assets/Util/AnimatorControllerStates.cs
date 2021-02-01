using System;
using System.Collections;
using System.Collections.Generic;
using System.IO; 
using UnityEngine;

namespace LSB
{
    public class AnimatorControllerStates : MonoBehaviour
    {
        private List<string> AnimationList = new List<string>();
        void Start()
        { 
            TextAsset codes = (TextAsset) Resources.Load("Codes");
            
            char[] delimiters = new char[] { '\r', '\n' };
            foreach(string s in codes.text.Split(delimiters,StringSplitOptions.RemoveEmptyEntries))
            {               
                AnimationList.Add(s);
            }
        }

        public bool HasStateName(string stateName)
        { 
            foreach(string stateSelected in AnimationList)
            {
                if (stateSelected.Contains(stateName))
                {
                    return true;
                }
            }
            return false;  
        }

        public bool HasAllStateNames(List<string> stateNames)
        {
            foreach(string stateName in stateNames)
            {
                if(!HasStateName(stateName))
                {
                    return false;
                }
            }
            return true;
        }

        public bool StateHasAnimationClip(Animator animator, Expression expression)
        {
            try
            { 
                string expressionCode = expression.getList().Substring(1);
                return HasAllStateNames(expression.code) && ExistAnimationOfExpression(animator, expressionCode);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ExistAnimationOfExpression(Animator animator, string expressionCode)
        {
            if (GetAnimationClip(animator, expressionCode))
            {
                return true;
            }
            return false;
        }

        public AnimationClip GetAnimationClip(Animator animator, string code)
        {
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name.Contains(code))
                {
                    return clip;
                }
            }
            return null;
        }

        public List<string> GetAnimationList()
        {
            return AnimationList;
        }
    }
}
