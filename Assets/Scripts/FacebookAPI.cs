using UnityEngine;
using System.Collections;
using Facebook.MiniJSON;
using System.Collections.Generic;

public class FacebookAPI : MonoBehaviour {

	private static FacebookAPI instance;

	protected string lastResponse = "";

	public void Start(){
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
			CallFBInit();
		}
		DontDestroyOnLoad(this.gameObject);
	}

	//facebook api
	
	private void CallFBInit()
	{
		FB.Init(OnInitComplete, OnHideUnity);
	}
	
	private void OnInitComplete()
	{
		//
	}
	
	private void OnHideUnity(bool isGameShown)
	{
		//Debug.Log("Is game showing? " + isGameShown);
	}
	
	public void CallFBLogin()
	{
		FB.Login("user_games_activity,user_friends,publish_actions", LoginCallback);
	}
	
	void LoginCallback(FBResult result)
	{
		if (result.Error != null)
			lastResponse = "Error Response:\n" + result.Error;
		else if (!FB.IsLoggedIn)
		{
			lastResponse = "Login cancelled by Player";
		}
		else
		{
			lastResponse = "Login was successful!";
		}
		//Debug.Log(lastResponse);
	}

	void Callback(FBResult result)
	{
		if (result.Error != null)
			lastResponse = "Callback Response:\n" + result.Error + "\n" + result.Text;
		else if (!FB.IsLoggedIn)
		{
			lastResponse = "Callback cancelled by Player";
		}
		else
		{
			lastResponse = "Callback was successful!";
		}
		//Debug.Log(lastResponse);
	}
	

	private void CallFBLogout()
	{
		FB.Logout();
	}

	//scoreboard

	public void createScore(int score){
		if(FB.IsLoggedIn){
			Dictionary<string,string> data = new Dictionary<string, string>();
			data["score"] = score.ToString();
			FB.API("/me/scores",Facebook.HttpMethod.POST,Callback,data);
		} else {
			Debug.Log ("Not logged in!");
		}
	}

	// to be removed
	public  void checkPermissions(){
		if(FB.IsLoggedIn){
			FB.API("/me/permissions", Facebook.HttpMethod.GET, delegate (FBResult response) {
				Debug.Log("Your permissions : \n" + response.Text);
			});
		}
	}

}
