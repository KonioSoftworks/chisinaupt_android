using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Police : MonoBehaviour {

	private GameObject player;
	private PlayerController pc;
	private CarController cc;

	private float ellapsedTime = 0f;
	private bool amendat = false;

	private float[] aPos = {-5f,-1.8f};
	private int amenda = 20;

	public GameObject ggUIImage; 
	public GameObject ggUIText;

	private float ttime;
	public bool activeRadar = false;

	public void getPlayer(){
		player = GameObject.FindGameObjectWithTag("Player");
		pc = player.GetComponent<PlayerController>();
		cc = this.GetComponent<CarController>();

	}

	// Update is called once per frame
	void Update () {
		if(!player)
			getPlayer();
		if(!player)
			return;
		ellapsedTime += Time.deltaTime;
		if((transform.position.z - player.transform.position.z < 10f) && (transform.position.z - player.transform.position.z > 0) && !amendat && (Mathf.RoundToInt(pc.getVelocity() * 2.6f) > 50)){
			amendat = true;
			int tot = (Mathf.RoundToInt((Mathf.RoundToInt(pc.getVelocity() * 2.6f) - 50)/20f)+1) * amenda;
			pc.money = Mathf.Max(0,pc.money-tot);
			ggUIText.SetActive(true); 
			ggUIText.GetComponent<Text>().text = "Speed limit exceeded : -"+tot+" lei";
			Invoke ("disableText",3f);
		}
		if(ellapsedTime >= Random.Range(20f,50f) && transform.position.z < player.transform.position.z - 10f){
			InitCar();
		} else if(transform.position.z < player.transform.position.z - 20f) {
			ggUIImage.SetActive(false);
			activeRadar = false;
		}

	}

	private void InitCar(){
		activeRadar = true;
		ggUIImage.SetActive(true);
		ellapsedTime = 0f;
		amendat = false;
		transform.position = new Vector3(aPos[Random.Range(0,1)],0,player.transform.position.z + 150f);
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Car"){
			if((transform.position.z < other.gameObject.transform.position.z && transform.position.x > 0) || 
			   (transform.position.z > other.gameObject.transform.position.z && transform.position.x < 0)){
				cc.setVelocity(Mathf.Abs(other.gameObject.rigidbody.velocity.z) - Random.Range(1f,3f));				
				CarController second = other.gameObject.GetComponent<CarController>();
				second.setVelocity(Mathf.Abs(other.gameObject.rigidbody.velocity.z) + Random.Range(1f,3f));
			}
		}
	}

	private void disableText(){
		ggUIText.SetActive(false);
	}

}
