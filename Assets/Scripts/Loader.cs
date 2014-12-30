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
	public AudioListener audioListener;

	public void loadAndExecute() {
		GoogleAD.hideAd();
		if(!buses[saveController.data.bus])
			saveController.data.bus = 0;
		spawnBus();
	}

	private void spawnBus(){
		Debug.Log("Spawned");
		Vector3 position = new Vector3(5f,0f,10f);
		GameObject bus1 = GameObject.FindGameObjectWithTag("Player");
		if(bus1){			
			Destroy(bus1);
		}
		transport = buses [saveController.data.bus];
		GameObject selBus = transport.car;
		bus = (GameObject)Instantiate(selBus,position,selBus.transform.rotation);
		pc = bus.GetComponent<PlayerController>();
		pc.coinValue = transport.coinValue;
	}

	public void Start(){
		saveController = new PlayerSave();
		Time.timeScale = 1;
		if(Application.loadedLevel == 2)
			loadAndExecute();
		refreshSoundButton();
		if(saveController.data.mute){
			audioListener.enabled = false;
			foreach(AudioSource source in audioSources){
				if(source){
					source.volume = 0;
					source.Stop();
					source.enabled = false;
				}
			}
		}
	}

	public void exitScene(){
		Application.LoadLevel("menu");
	}

	public void Died(int score){
		saveController.data.money += score;
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
		audioListener.enabled = !saveController.data.mute;
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
