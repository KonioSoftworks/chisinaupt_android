using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public void Start(){
		GoogleAD.showAd(true);
	}

	public void goFacebook(){
		Application.OpenURL("https://www.facebook.com/koniosoftworks");
	}

	public void Exit(){
		Application.Quit();
	}
}
