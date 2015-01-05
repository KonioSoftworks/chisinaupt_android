using UnityEngine;
using System.Collections;


public class Menu : MonoBehaviour {

	public FacebookAPI FBApi;

	public GameObject facebookConnectButton;

	public void Start(){
		GoogleAD.showAd(true);
		FBAction();
	}

	public void Update(){
		FBAction();
	}

	public void Exit(){
		Application.Quit();
	}

	private void FBAction(){
		if(FB.IsLoggedIn){
			facebookConnectButton.SetActive(false);
		}
	}

	public void ConnectToFacebook(){
		FBApi.CallFBLogin();
	}

}
