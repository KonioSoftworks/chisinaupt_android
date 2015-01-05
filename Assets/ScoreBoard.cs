using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Facebook.MiniJSON;
using System.Collections.Generic;

public class ScoreBoard : MonoBehaviour {
	
	public GameObject localHighscore;
	public GameObject facebookHighscore;
	public FacebookAPI FBApi;
	private PlayerSave saveController;
	//private Dictionary<string,object> scores = new Dictionary<string, object>();
	private List<object> scores;
	public void Start(){
		if(!FB.IsLoggedIn)
			facebookHighscore.GetComponent<Text>().text = "You're not connected to Facebook. ";
		else{
			getHighScores();
		}
		saveController = new PlayerSave();
		localHighscore.GetComponent<Text>().text = "Your highscore : "+ saveController.data.localHighscore + " lei";
	}

				
	private void getHighScores(){
		FB.API("/"+FB.AppId+"/scores",Facebook.HttpMethod.GET,onEnd);
	}
	
	private void onEnd(FBResult result){
		if(result.Error != null)
			facebookHighscore.GetComponent<Text>().text = "Something wrong here...";
		else {
			scores = DeserializeScores(result.Text);
			int i = 1;
			foreach(object score in scores) 
			{
				var entry = (Dictionary<string,object>) score;
				var user = (Dictionary<string,object>) entry["user"];
				
				string userId = (string)user["id"];
				if(i > 1)
					facebookHighscore.GetComponent<Text>().text += "\n--------------------------------------------------";
				if (string.Equals(userId,FB.UserId))
				{
					facebookHighscore.GetComponent<Text>().text += "\n"+ i + "\tYou ".ToUpperInvariant()+entry["score"] + " lei".ToUpperInvariant();
				} else {
					facebookHighscore.GetComponent<Text>().text += "\n"+ i + "\t" + user["name"]+ " " +entry["score"]+ " lei";
				}
		
				Debug.Log ("Parsed");
				i++;
			}
			//facebookHighscore.GetComponent<Text>().text = o;

		}
	}

	public static List<object> DeserializeScores(string response) 
	{
		
		var responseObject = Json.Deserialize(response) as Dictionary<string, object>;
		object scoresh;
		var scores = new List<object>();
		if (responseObject.TryGetValue ("data", out scoresh)) 
		{
			scores = (List<object>) scoresh;
		}
		
		return scores;
	}

}
