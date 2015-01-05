using UnityEngine;
using System.Collections;

public class TouchController : MonoBehaviour {

	GameObject player;
	PlayerController playerController;

	private bool brake = false;
	private bool gas = false;

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
		playerController.touchGas = gas;
	}

	public void onClick(int dir){
		playerController.makeTouch(dir);
	}
	
	public void setBrake(bool b){
		brake = b;
	}

	public void setGas(bool s){
		gas = s;
	}

	public void accessMenu(){
		playerController.menuTouch();
	}

}
