using UnityEngine;
using System.Collections;

public class TouchController : MonoBehaviour {

	GameObject player;
	PlayerController playerController;

	private bool brake = false;

	public void Start(){
		player = GameObject.FindGameObjectWithTag("Player");
		if(!player){
			Debug.LogWarning("Touch System not working");
			return;
		}
		playerController = player.GetComponent<PlayerController>();
	}

	public void Update(){
		if(brake)
			playerController.brakeTouch();
	}

	public void onClick(int dir){
		playerController.makeTouch(dir);
	}

	public void brakeIn(){
		brake = true;
	}

	public void brakeOut(){
		brake = false;
	}

	public void accessMenu(){
		playerController.menuTouch();
	}

}
