using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour{
	GameObject lImage;
	string scene;

	private bool activated = false;
	private int click = 0;

	public void Awake(){
		getO ();
		scene = "";
		Time.timeScale = 1f;
	}

	public void Update(){

		if(!activated)
			return;
		click++;
		if(click > 1){
			Application.LoadLevel(scene);
		}
	}

	public void loadScene(string sc){
		activated = true;
		scene = sc;
		if(!lImage)
			getO ();
		if(lImage)
			lImage.SetActive(true);
	}

	private void getO(){		
		lImage = GameObject.FindGameObjectWithTag("ImageLS");
		if(lImage)
			lImage.SetActive(false);
	}
	
}