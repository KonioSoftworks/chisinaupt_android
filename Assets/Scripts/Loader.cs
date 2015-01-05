using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.UI;


public class Loader : MonoBehaviour {

	public List<AudioSource> audioSources;

	public List<Transport> buses;
		
	public PlayerSave saveController;

	private GameObject bus;
	private PlayerController pc;

	public Transport transport;

	private int score = 0;

	public GameObject deadText;

	//sound
	public GameObject toggleSoundOn;
	public GameObject toggleSoundOff;

	//analytics
	public GoogleAnalyticsV3 googleAnalytics;
	public FacebookAPI FBApi;

	public void loadAndExecute() {
		GoogleAD.hideAd();
		if(!buses[saveController.data.bus])
			saveController.data.bus = 0;
		spawnBus();
	}

	private void spawnBus(){
		Vector3 position = new Vector3(5f,0f,10f);
		GameObject bus1 = GameObject.FindGameObjectWithTag("Player");
		if(bus1){			
			Destroy(bus1);
		}
		transport = buses [saveController.data.bus];
		GameObject selBus = transport.car;
		bus = (GameObject)Instantiate(selBus,position,selBus.transform.rotation);
		if(saveController.data.mute){
			bus.audio.enabled = false;
		}
		pc = bus.GetComponent<PlayerController>();
		pc.coinValue = transport.coinValue;
	}

	public void Start(){
		saveController = new PlayerSave();
		if(FB.IsLoggedIn && !saveController.data.highscoreSent){
			FBApi.createScore(saveController.data.localHighscore);
			saveController.data.highscoreSent = true;
			saveController.Save();
		}
		Time.timeScale = 1;
		if(Application.loadedLevel == 2)
			loadAndExecute();
		refreshSoundButton();
		if(saveController.data.mute){
			foreach(AudioSource source in audioSources){
				if(source){
					source.volume = 0;
					source.Stop();
					source.enabled = false;
				}
			}
		}
		if(googleAnalytics){
			googleAnalytics.LogScreen(Application.loadedLevelName);
		}
	}

	public void exitScene(){
		Application.LoadLevel("menu");
	}

	public void Died(int score){
		if(googleAnalytics){
			googleAnalytics.LogEvent(new EventHitBuilder()
			                         .SetEventCategory("Cursa")
			                         .SetEventAction("Sfirsit cursa")
			                         .SetEventLabel("Cistig")
			                         .SetEventValue(score));
			googleAnalytics.LogEvent(new EventHitBuilder()
			                         .SetEventCategory("Cursa")
			                         .SetEventAction("Sfirsit cursa")
			                         .SetEventLabel("Distanta")
			                         .SetEventValue(Mathf.RoundToInt(bus.transform.position.y)));
		}
		saveController.data.money += score;
		saveController.data.localHighscore = Mathf.Max(saveController.data.localHighscore,score);
		if(score == saveController.data.localHighscore){
			//send to facebook
			if(FB.IsLoggedIn)
				FBApi.createScore(score);
			else
				saveController.data.highscoreSent = false;
		}
		saveController.Save();
		pc.audio.Stop();
		if(deadText){
			deadText.GetComponent<Text>().text = "Măăăi...\nYou earned "+score+" lei";
		}
	}

	public void resumeButton(){
		pc.Resume();
	}

	public void retryButton(){
		pc.Save();
		Application.LoadLevel("scene");
	}
		
	public void toggleSound(){
		saveController.data.mute = !saveController.data.mute;
		foreach(AudioSource source in audioSources){
			if(source){
				if(saveController.data.mute){
					source.volume = 0;
					source.Stop();
					source.enabled = false;
				}else {
					source.enabled = true;
					source.volume = 1;
					source.Play();
				}
			}
		}
		saveController.Save();
		refreshSoundButton();
	}

	private void refreshSoundButton(){
		MusicPlayer.isMute = saveController.data.mute;
		if(toggleSoundOff && toggleSoundOn){
			if(saveController.data.mute){
				toggleSoundOn.SetActive(false);
				toggleSoundOff.SetActive(true);
			}else{
				toggleSoundOn.SetActive(true);
				toggleSoundOff.SetActive(false);
			}
		}
	}
	
}
