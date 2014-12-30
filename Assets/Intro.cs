using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Intro : MonoBehaviour {

	public List<GameObject> intr;

	private float ellapsedTime = 0f;
	private float timePerImage = 2f;
	private int index = 0;

	void Start(){
		if(intr.Count > 0)
			activateIntr(0);
	}

	void Update(){
		ellapsedTime += Time.deltaTime;
		if(ellapsedTime >= timePerImage){
			ellapsedTime = 0f;
			activateIntr(++index);
		}
		if(index >= intr.Count)
			Application.LoadLevel("menu");
	}

	private void activateIntr(int x){
		for(int i=0; i < intr.Count;i++){
			if(i == x)
				intr[i].SetActive(true);
			else
				intr[i].SetActive(false);
		}
	}

}
