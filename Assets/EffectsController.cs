using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EffectsController : MonoBehaviour {

	public Text data;
	public AudioSource coinsAs;

	private GameObject player;
	private PlayerController pcontroller;
	private int money;

	// Use this for initialization
	void Awake() {
		findPlayerIfNotExist();
		money = 0;
	}
	
	// Update is called once per frame
	void Update () {
		findPlayerIfNotExist();
		if (pcontroller) {
			if(money != pcontroller.money)
				coinEffect();
			data.text = pcontroller.money.ToString() + " lei\n" +  (player.transform.position.z/1000f).ToString("F2") +" km\n" + Mathf.RoundToInt(pcontroller.getVelocity() * 2.6f)	.ToString() + "km/h";
		}
	}

	void findPlayerIfNotExist(){
		if(player)
			return;
		player = GameObject.FindGameObjectWithTag ("Player");
		if(player){
			pcontroller = player.GetComponent<PlayerController>();
		}
	}

	public void coinEffect(){
		if(coinsAs.enabled)
			coinsAs.Play();
		money = pcontroller.money;
	}

}
