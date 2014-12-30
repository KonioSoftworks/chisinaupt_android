using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadGeneratorScript : MonoBehaviour {

	public List<GameObject> AvailablePlanes;

	private GameObject Road;

	public float distanceFromCar = 10f;
	private float distance = 0f;
	private float distanceToNextPlane = 10f;
	private float distanceCoins = 0f;

	//buildings
	public List<GameObject> availableBuildings;
	private List<GameObject> buildings;
	private float leftDistance = 0f;
	private float rightDistance = 0f;
	private int CoinsInRow = 0;

	//COINS RENDERING OPTIONS

	private int maxCoinsInRow = 7;
	private int minCoinsInRow = 1;

	//tiles
	public const int renderedPlanes = 15;
	private GameObject[] tiles;
	private GameObject player;
	private Loader loader;

	float[] bandsPositions = new float[]{-5f,-1.8f,1.8f,5f}; // coins positions


	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		Vector3 carV = player.transform.position;
		Road = GameObject.FindGameObjectWithTag("Road");		
		buildings = new List<GameObject>();

		//loading transport properties
		GameObject MC = GameObject.FindGameObjectWithTag ("MainController");
		if (MC) {
			loader = MC.GetComponent<Loader>();
			maxCoinsInRow = loader.transport.coinFreq;
		}



		tiles = new GameObject[renderedPlanes];
		for(int i=0;i < renderedPlanes;i++){
			createNewPlane(i);
		}
		while(distanceCoins - carV.z < 100){	
			CoinsInRow = Random.Range(minCoinsInRow,maxCoinsInRow);
			CoinRow(CoinsInRow,Random.Range(0,4));
		}
		renderBuildings();

	}

	void Update () {
		var Coins = GameObject.FindGameObjectsWithTag("Coins");
		Vector3 carV = player.transform.position;
		transform.position = new Vector3(transform.position.x,transform.position.y,carV.z - distanceFromCar);
		for(int i=0;i < renderedPlanes;i++){
			if(tiles[i].transform.position.z < carV.z - 1.5f*distanceToNextPlane){
				createNewPlane(i);
			}
		}
		CoinsInRow = Random.Range(minCoinsInRow,maxCoinsInRow);
		if(distanceCoins - carV.z < 100)
			CoinRow(CoinsInRow,Random.Range(0,4));
		for(int i=0;i < Coins.Length;i++){
			if(Coins[i].transform.position.z < carV.z - distanceToNextPlane || !Coins[i].activeSelf){
				Destroy(Coins[i]);
			}
		}
		renderBuildings();
	}

	void createNewPlane(int nr){
		if(tiles[nr]){			
			GameObject plane = AvailablePlanes[Random.Range(0,AvailablePlanes.Count)];
			Destroy(tiles[nr]);
			tiles[nr] = Instantiate(plane,new Vector3(tiles[nr].transform.position.x,tiles[nr].transform.position.y,distance),plane.transform.rotation) as GameObject;
			distance += distanceToNextPlane;
		} else {
			GameObject plane = AvailablePlanes[Random.Range(0,AvailablePlanes.Count)];
			Vector3 newPosition = new Vector3(plane.transform.position.x,plane.transform.position.y,
			                                  distance);
			Quaternion rotation = new Quaternion(0,0,0,0); 
			distance += distanceToNextPlane;
			tiles[nr] = (GameObject)Instantiate(plane,newPosition,rotation);
			tiles[nr].transform.parent = Road.transform;
		}
	}

	void generateNewCoin(int band,ref float rowDistance){
		Vector3 position = new Vector3(bandsPositions[band], loader.transport.coinPrefab.transform.position.y,rowDistance);		
		GameObject newCoin = (GameObject)Instantiate(loader.transport.coinPrefab,position,loader.transport.coinPrefab.transform.rotation);
		newCoin.transform.Rotate(new Vector3(15,30,45) * Time.deltaTime*10);
		rowDistance += Random.Range(3,5);
	}

	void CoinRow(int n,int band){
		float position = distanceCoins;
		for(int i = 0; i<n; i++){
			generateNewCoin(band,ref position);
		}
		distanceCoins += Random.Range(10,50);
	}

	void renderBuildings() {
		for(int i=0;i < buildings.Count;i++) {
			if(buildings[i].transform.position.z + 10f < transform.position.z){
				Destroy(buildings[i]);
				buildings.RemoveAt(i);
			}
		}
		renderSide(ref leftDistance,-1);
		renderSide(ref rightDistance,1);
	}
	
	void renderSide(ref float sideDistance,int pos){
		while(sideDistance <= distance){
			GameObject obj = availableBuildings[Random.Range(0,availableBuildings.Count)];
			BuildingController objController = (BuildingController) obj.GetComponent(typeof(BuildingController));
			Vector3 position = new Vector3(20f*pos+pos*objController.distanceFromRoad,objController.heightFromRoad,sideDistance);			
			buildings.Add((GameObject)Instantiate(obj,position,obj.transform.rotation));
			sideDistance += objController.distanceFromOthers;
		}
	}

}
