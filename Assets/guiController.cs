using UnityEngine;
using System.Collections;

public class guiController : MonoBehaviour {

	public GameObject pausePanel;
	public GameObject deathPanel;

	private GameObject player;
	private PlayerController pc;

	private void getPlayer(){
		player = GameObject.FindGameObjectWithTag("Player");
		if(player)
			pc = player.GetComponent<PlayerController>();
		else
			Debug.Log("Player not found");
	}

	// Update is called once per frame
	void Update () {
		if(!player)
			getPlayer();
		if(pc.isPaused){
			pausePanel.SetActive(true);
			Time.timeScale = 0;
			GoogleAD.showAd(false);
			return;
		} else {
			pausePanel.SetActive(false);
			Time.timeScale = 1;
			GoogleAD.hideAd();
		}
		if(pc.gameOver){
			if(Time.timeScale > 0)
				pc.Save();
			deathPanel.SetActive(true);
			Time.timeScale = 0;
			GoogleAD.showAd();
		} else {
			deathPanel.SetActive(false);
			Time.timeScale = 1;
			GoogleAD.hideAd();
		}
	}
}
