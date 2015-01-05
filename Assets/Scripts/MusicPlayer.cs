using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicPlayer : MonoBehaviour {

	public List<AudioClip> sounds;
	private int songId = 0; 

	private static MusicPlayer instance;
	private GameObject player;

	public static bool isMute = false;
	public static bool wasMute = false;

	public static MusicPlayer GetInstance(){
		return instance;
	}
	
	void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
			if(sounds.Count > 0){
				songId = Random.Range(0,sounds.Count);
				audio.clip = sounds[songId];
				audio.loop = false;	
				audio.Play();
			}
		}
		DontDestroyOnLoad(this.gameObject);
	}

	void Update(){
		if(isMute && !wasMute){
			audio.Pause();
			wasMute = true;
		}
		if(!isMute && wasMute){
			audio.Play();
			wasMute = false;
		}
		if(isMute)
			return;

		getPlayerAndMove();
		if(!audio.isPlaying){
			songId++;
			if(songId >= sounds.Count)
				songId = 0;
			audio.clip = sounds[songId];
			audio.Play();
		}
	}

	void getPlayerAndMove(){
		if(player == null && Application.loadedLevel == 2){
			player = GameObject.FindGameObjectWithTag("Player");
		}
		if(player != null){
			this.transform.position = new Vector3(player.transform.position.x,0,player.transform.position.z+10f);
		}
	}
}
