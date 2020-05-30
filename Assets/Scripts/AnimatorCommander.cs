using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LSB
{
    public class AnimatorCommander : MonoBehaviour
    {
        public Animator anim;
        public AnimatorControllerStates controller;
        public Text mainText;

        public float animationDuration;
        public float animationSpeed;

        private static float DEFAULT_SPEED = 1.0f;

        public void start()
        {
            animationDuration = 1.5f;
            animationSpeed = 1.0f;
        }

        public void setSlowSpeed()
        {
            animationDuration = 1.5f;
            animationSpeed = 1.0f;
        }

        public void setMediumSpeed()
        {
            animationDuration = 1.0f;
            animationSpeed = 1.5f;
        }

        public void setFastSpeed()
        {
            animationDuration = 0.5f;
            animationSpeed = 3.0f;
        }

        public void OnCommand(ExpressionList expressions)
        {
            Debug.Log("STARTCOROUTINE");
            StartCoroutine(scene(expressions));
        }

        public void OnError(string word)
        {
            Debug.Log("LLEGO AQUI ERROR");
            Debug.Log("ERROR: " + word);
            ExpressionList expressions = LocalParser.parseExpressionList(word);
            Debug.Log(expressions.tokens.Count);
            StartCoroutine(scene(expressions));
        }

        public IEnumerator scene(ExpressionList expressions)
        {
            anim.speed = animationSpeed; 
            
            foreach (Expression expression in expressions.tokens)
            {
                if(!expression.getList().Contains("#99"))
		        {
		            mainText.text = expression.word;
                    Debug.Log("TIEMPO: "+mainText.text);
		        }
		        Expression selected = expression;
                if(!controller.hasAllStateNames(expression.code) || (controller.hasAllStateNames(expression.code) && !existAnimationOfExpression(expression.getList().Substring(1))))
                {
                    selected = LocalParser.parseExpression(expression.word);
                }
                foreach(string code in selected.code)
                {
                    anim.SetInteger("currentSign", int.Parse(code.Substring(1)));
                     
                    AnimationClip clip = GetAnimationClip(code.Substring(1));
                    if (clip)
                    {
                        animationDuration = clip.length; 
                    }
                    Debug.Log("ANIMATION DURATION: " + animationDuration + " -- ANIMATION SPEED: " + animationSpeed); 
                    yield return new WaitForSeconds(animationDuration); 
                }
            }
            anim.speed = DEFAULT_SPEED;
            anim.SetInteger("currentSign", 0);
            mainText.text = "";
        }

        private bool existAnimationOfExpression(string codeExpression)
        {
            if (GetAnimationClip(codeExpression))
            {
                return true;
            }
            return false;
        }

        public AnimationClip GetAnimationClip(string code)
        {  
            AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name.Contains(code))
                {
                    return clip; 
                }
            }
            return null;
        }

    }
}
