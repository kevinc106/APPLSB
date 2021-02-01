using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using CreationStates;
using System.Linq;
// Create a menu item that causes a new controller and statemachine to be created.
public class StateMachine : MonoBehaviour
{
    static string UNITY_RESOURCES_FOLDER = Path.Combine("Assets","Resources");
    static string CATEGORY_PATH = Path.Combine(UNITY_RESOURCES_FOLDER,"DataBase","config","categories.json");
    static string CONFIG_PATH = Path.Combine(UNITY_RESOURCES_FOLDER,"DataBase","config","data.json");
    static string NAME_ANIMATOR_CONTROLLER = "main.controller";
    static string CODE_INDICATOR = "#";
    static string ANIMATIONS_FOLDER_PATH = @"Animations";
    static string CODE_SEPARATOR_CLIP = "_";
    static string NAME_CODES_FILE = "Codes.txt";
    static Database datadase;
    static List<object> expressionCodes = new List<object>();  
    static AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(Path.Combine("Assets", NAME_ANIMATOR_CONTROLLER));
    
    static List<AnimatorStateTransition> currentTransitions = new List<AnimatorStateTransition>();

    [MenuItem("Menu Controller LSB/Create New Controller")]
    static void CreateController()
    {
        try
        {
            datadase = new Database(CATEGORY_PATH, CONFIG_PATH);
            expressionCodes = datadase.Expressions;
            if (controller==null)
            {
                // Creates the controller
                string pathNewController = Path.Combine("Assets", NAME_ANIMATOR_CONTROLLER);
                controller = AnimatorController.CreateAnimatorControllerAtPath(pathNewController);

                // Add parameters
                AnimatorControllerParameter animationSpeedParameter = new AnimatorControllerParameter();
                animationSpeedParameter.name = "animationSpeed";
                animationSpeedParameter.type = AnimatorControllerParameterType.Float;
                animationSpeedParameter.defaultFloat = 1.0f;
                controller.AddParameter("currentSign", AnimatorControllerParameterType.Int);
                controller.AddParameter(animationSpeedParameter);
                var rootStateMachine = controller.layers[0].stateMachine;
                controller.layers[0].stateMachine.name = "Base Layer"; 

                // Add States  
                //SetState("MainIdle");
                AnimatorState newState = GetNewState(rootStateMachine, "MainIdle");
                newState.motion = GetMainIdleAnimation(); 
                Debug.Log("MaindIdle state created"); 
                AddToCurrentTransitions(newState);
            }
            else
            {
                Debug.Log("Animator controller "+controller.name+" already exist");
            }
        }
        catch (Exception error)
        {
            Debug.Log("CreateController ERROR: " + error.Message+" -- "+error.Source);
        }
    }

    static AnimationClip GetMainIdleAnimation()
    {
        var animationClips = Resources.LoadAll(ANIMATIONS_FOLDER_PATH);  
        foreach (AnimationClip clip in animationClips)
        {
            if (clip.name.Contains("MainIdle"))
            {
                return clip;
            }
        }
        return null;
    } 
     
    
    static bool ExistState(string stateName)
    {
        try
        {
            var rootStateMachine = controller.layers[0].stateMachine; 
            List <ChildAnimatorState> animatorStates = new List<ChildAnimatorState>(rootStateMachine.states); 
            foreach (var animatorState in animatorStates)
            {
                if (animatorState.state.name == stateName)
                {
                    return true;
                }
            }
        }
        catch(Exception error)
        {
            Debug.Log("isAlreadyState ERROR: " + error.Message + " -- " + error.Source);
        } 
        return false;
    }

    [MenuItem("Menu Controller LSB/1. Load States")]
    static void LoadAllStates()
    { 
        var rootStateMachine = controller.layers[0].stateMachine;
        datadase = new Database(CATEGORY_PATH, CONFIG_PATH);
        expressionCodes = datadase.Expressions;
        foreach (ExpressionData expression in expressionCodes)
        {     
            if (!ExistState(expression.Code[0]))
            {
                AnimatorState newState = GetNewState(rootStateMachine, expression.Code[0]); 
                AddToCurrentTransitions(newState);
            }
            else
            { 
                Debug.Log("State: " + expression.Code[0] + " has already created");
            } 
        }
        AddStatesInCodesFile(expressionCodes);
    }

    private static void AddStatesInCodesFile(List<object> expressions)
    {
        /*var rootStateMachine = controller.layers[0].stateMachine;
        List<ChildAnimatorState> animatorStates = new List<ChildAnimatorState>(rootStateMachine.states);
        List<string> stateCodes = new List<string>();
        foreach (var animatorState in animatorStates)
        {
            stateCodes.Add(animatorState.state.name);
        } */
        List<string> stateCodes = new List<string>();
        stateCodes.Add("MainIdle");
        foreach (ExpressionData expression in expressions)
        {
            stateCodes.Add(expression.Expression+expression.Code[0]);
        }
        File.WriteAllLines(Path.Combine("Assets", "Resources",NAME_CODES_FILE), stateCodes,System.Text.Encoding.UTF8); 
    }

    private static AnimatorState GetNewState(AnimatorStateMachine rootStateMachine, string code)
    {
        AnimatorState newState = rootStateMachine.AddState(code);
        newState.speedParameterActive = true;
        newState.speedParameter = "animationSpeed";
        return newState;
    }

    static void AddToCurrentTransitions(AnimatorState destinationState)
    {  
        currentTransitions.Add(GetAnimatorStateTransition(destinationState)); 
    }

    static AnimatorStateTransition GetAnimatorStateTransition(AnimatorState destinationAnimatorState){
        try
        {
            AnimatorStateTransition transition = new AnimatorStateTransition
            {
                destinationState = destinationAnimatorState,
                hasExitTime = false,
                exitTime = float.Parse("0,75"),
                hasFixedDuration = true,
                duration = float.Parse("0,5"),
                canTransitionToSelf = false
            };
            String currentSign = "";
            if (destinationAnimatorState.name == "MainIdle")
            {
                currentSign = "MainIdle";
                transition.AddCondition(AnimatorConditionMode.Equals, 0, "currentSign");
            }
            else
            {
                currentSign = destinationAnimatorState.name.Substring(1);
                transition.AddCondition(AnimatorConditionMode.Equals, int.Parse(currentSign), "currentSign");
            }
            return transition;
        }
        catch (Exception error)
        {
            Debug.Log("LoadAnimations ERROR: " + error.Message + " -- " + error.Source);
        }
        return null;
    }


    [MenuItem("Menu Controller LSB/2. Load Transitions")]
    static void LoadAllTransitions()
    {
        try
        {   
            var rootStateMachine = controller.layers[0].stateMachine;
            ChildAnimatorState[] animatorStatesList = rootStateMachine.states;
            if (rootStateMachine.anyStateTransitions.Length<1)
            {
                ReloadAllTransitions(animatorStatesList);
            }
            else
            {
                List<AnimatorStateTransition> oldTransitions = GetOldTransitionsList(rootStateMachine);
                oldTransitions.AddRange(currentTransitions);
                rootStateMachine.anyStateTransitions = oldTransitions.ToArray();
            }
            rootStateMachine.anyStateTransitions = currentTransitions.ToArray();
            Debug.Log("Transitions reloaded");
        }
        catch(Exception error)
        {
            Debug.Log("loadAllTransitions ERROR: " + error.Message + " -- " + error.Source);
        }
    }

    private static List<AnimatorStateTransition> GetOldTransitionsList(AnimatorStateMachine rootStateMachine)
    {
        return rootStateMachine.anyStateTransitions.OfType<AnimatorStateTransition>().ToList();
    }

    static void ReloadAllTransitions(ChildAnimatorState[] animatorStatesList)
    {
        currentTransitions = new List<AnimatorStateTransition>();
        foreach (var animatorState in animatorStatesList)
        {
            AddToCurrentTransitions(animatorState.state);
        } 
    }
 

    [MenuItem("Menu Controller LSB/3. Load All Animations")]
    static void LoadAnimations()
    {
        datadase = new Database(CATEGORY_PATH, CONFIG_PATH);
        expressionCodes = datadase.Expressions;
        var animationClips = Resources.LoadAll(ANIMATIONS_FOLDER_PATH);
            Debug.Log("Clips Founded: " + animationClips.Length);
            var rootStateMachine = controller.layers[0].stateMachine; 
            ChildAnimatorState[] animatorStatesList = rootStateMachine.states;
            foreach (AnimationClip clip in animationClips)
            {
                if (clip.name.Contains(CODE_SEPARATOR_CLIP))
                {
                    String clipCode = clip.name.Substring(clip.name.Length - 5);
                    //if exist code clip in data base
                    bool firstFlag = false; bool secondFlag = false; 
                    foreach (ExpressionData expression in expressionCodes) {
                        if (expression.Code[0].Contains(CODE_INDICATOR + clipCode))
                        {
                            firstFlag = true;
                            foreach (var animatorState in animatorStatesList)
                            {
                                if ("#" + clipCode == animatorState.state.name)
                                {
                                    secondFlag = true;
                                    animatorState.state.motion = clip;
                                    break;
                                } 
                            }
                            if (secondFlag == false)
                            {
                                Debug.Log("Clip name " + clipCode + " doesn't have state");
                            }
                            break;
                        }
                    }
                    if(firstFlag == false) 
                    {
                        Debug.Log("Clip name " + clipCode + " doesn't exist in database");
                    }
                }
                else
                { 
                    Debug.Log("Clip has errors in name: " + clip.name); 
                }
            } 
        
    } 
}
