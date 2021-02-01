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
        private string OMITTED_CATEGORY = "#99";
        private string CONDITIONAL_EVENT_PARAMETER = "currentSign";
        public void Start()
        {
            //animationDuration = 1.5f;
            animationSpeed = 1.0f;
        } 

        public void SetSlowSpeed()
        {
            //animationDuration = 1.5f;
            animationSpeed = 1.0f;
        }

        public void SetMediumSpeed()
        {
            //animationDuration = 1.0f;
            animationSpeed = 1.5f;
        }

        public void SetFastSpeed()
        {
            //animationDuration = 0.5f;
            animationSpeed = 3.0f;
        }

        public void OnCommand(ExpressionList expressions)
        { 
            StartCoroutine(scene(expressions));
        }

        public void OnError(string word)
        {
            ExpressionList expressions = LocalParser.ParseExpressionList(word);
            Debug.Log(expressions.tokens.Count);
            StartCoroutine(scene(expressions));
        }

        public IEnumerator scene(ExpressionList expressions)
        {
            anim.speed = animationSpeed; 
             
            foreach (Expression expression in expressions.tokens)
            {
                if (!expression.getList().Contains(OMITTED_CATEGORY))
                {
                    mainText.text = expression.word;
                }
                
		        Expression selected = expression;
                if(!controller.HasAllStateNames(expression.code) || !controller.StateHasAnimationClip(anim,expression))
                {
                    selected = LocalParser.ParseExpression(expression.word);
                }
                foreach(string code in selected.code)
                {
                    anim.SetInteger(CONDITIONAL_EVENT_PARAMETER, int.Parse(code.Substring(1)));
                     
                    AnimationClip clip = controller.GetAnimationClip(anim,code.Substring(1));
                    if (clip)
                    {
                        animationDuration = clip.length; 
                    }
                      
                    yield return new WaitForSeconds(animationDuration); 
                }
            }
            anim.speed = DEFAULT_SPEED;
            anim.SetInteger(CONDITIONAL_EVENT_PARAMETER, 0);
            mainText.text = "";
        }
         
    }
}
