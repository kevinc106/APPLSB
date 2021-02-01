using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class Toast : MonoBehaviour {
	float _counter = 0f;
	float _duration;
	bool _isToasting = false;
	bool _isToastShown = false;

	public static Toast Instance;
	[SerializeField] Text toastText;
	[SerializeField] Animator anim;
	[SerializeField] Color[] co;
	Image toastColorImage;

	public enum ToastColor{Dark,Red,Green,Blue,Magenta,Pink}

	void Awake () {Instance = this;}

	void Start () {toastColorImage = GetComponent <Image> ();}

	void Update(){
		if (_isToasting){
			if (!_isToastShown){
				toastShow ();
				_isToastShown = true;
			}
			_counter += Time.deltaTime;
			if(_counter>=_duration){
				_counter = 0f;
				_isToasting = false;
				toastHide ();
				_isToastShown = false;
			}
		}
	}

 
	public void Show(){
		toastColorImage.color = co [0];
		toastText.text = "Hello ;)";
		_duration = 1f;
		_counter = 0f;
		if (!_isToasting) _isToasting = true;
	}
	 
	public void Show(string text){
		toastColorImage.color = co [0];
		toastText.text = text;
		_duration = 1f;
		_counter = 0f;
		if (!_isToasting) _isToasting = true;
	}

 
	public void Show(string text, float duration){
		toastColorImage.color = co [0];
		toastText.text = text;
		_duration = duration;
		_counter = 0f;
		if (!_isToasting) _isToasting = true;
	}
 
	public void Show(string text, float duration, ToastColor color){
		toastColorImage.color = co [0];
		toastColorImage.color = co [(int)color];
		toastText.text = text;
		_duration = duration;
		_counter = 0f;
		if (!_isToasting) _isToasting = true;
	} 
	 
	void toastShow(){anim.SetBool ("isToastUp",true);}
	void toastHide(){anim.SetBool ("isToastUp",false);}
}
