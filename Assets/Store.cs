using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Store : MonoBehaviour {

	//info data
	public GameObject cash;
	public GameObject cost;
	public GameObject infobox;

	//controllers
	public GameObject selectButton;
	public GameObject buyButton;

	public Loader mainController;

	public GameObject scene;

	private int currentCar = 0;

	//analytics
	public GoogleAnalyticsV3 googleAnalytics;

	// Use this for initialization
	void Awake () {
		mainController.saveController = new PlayerSave();
		updateCash();
		currentCar = mainController.saveController.data.bus;
		spawnCar(currentCar);
		GoogleAD.showAd();
	}

	public void goBack(){
		Application.LoadLevel("menu");
	}

	private void updateCash(){
		cash.GetComponent<Text>().text = "Cash : " + mainController.saveController.data.money + " lei";
	}

	private void spawnCar(int id){
		GameObject car = GameObject.FindGameObjectWithTag("Player");
		if(car)
			Destroy(car);
		GameObject carGO = mainController.buses[id].car;
		if(!carGO){
			Debug.Log("Bus not found ;((((");			
			carGO = mainController.buses[0].car;
		}
		car = (GameObject)Instantiate(carGO,Vector3.zero,carGO.transform.rotation);
		car.GetComponent<PlayerController>().enabled = false;
		car.transform.parent = scene.transform;
		car.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		if(mainController.saveController.data.mute){
			car.audio.volume = 0;
			car.audio.Stop();
			car.audio.enabled = false;
		}

		//updating data

		cost.GetComponent<Text>().text = "Cost : " + mainController.buses[id].price + " lei";
		infobox.GetComponent<Text>().text = "";
		foreach(string text in mainController.buses[id].info){
			infobox.GetComponent<Text>().text += (char)183 + " " + text + (char)10;
		}
		if(haveBus(currentCar)){
			buyButton.SetActive(false);
			selectButton.SetActive(true);
		} else {
			buyButton.SetActive(true);
			selectButton.SetActive(false);
		}
		if(mainController.saveController.data.bus == currentCar)
			selectButton.SetActive(false);
	}

	private bool haveBus(int id){
		bool a = false;
		for(int i=0;i<mainController.saveController.data.ownBuses.Count;i++){
			if(mainController.saveController.data.ownBuses[i] == id)
				a = true;
		}
		return a;
	}

	private int getMin(int pivot){
		bool found = false;
		int pos = 0;
		for(int i=0;i < mainController.buses.Count;i++){
			if(mainController.buses[i].price > mainController.buses[pivot].price){
				if(!found)
					pos = i;
				else
					if(mainController.buses[i].price < mainController.buses[pos].price)
						pos = i;
				found = true;
			}
		}
		if(found)
			return pos;
		else {
			pos = 0;			
			for(int i=0;i < mainController.buses.Count;i++){
				if(mainController.buses[i].price < mainController.buses[pos].price)
					pos = i;
			}
			return pos;
		}
	}

	private int getMax(int pivot){
		bool found = false;
		int pos = 0;
		for(int i=0;i < mainController.buses.Count;i++){
			if(mainController.buses[i].price < mainController.buses[pivot].price){
				if(!found)
					pos = i;
				else
					if(mainController.buses[i].price > mainController.buses[pos].price)
						pos = i;
				found = true;
			}
		}
		if(found)
			return pos;
		else {
			pos = 0;			
			for(int i=0;i < mainController.buses.Count;i++){
				if(mainController.buses[i].price > mainController.buses[pos].price)
					pos = i;
			}
			return pos;
		}
	}

	public void nextCar(){
		currentCar = getMin (currentCar);
		spawnCar(currentCar);
	}

	public void previousCar(){
		currentCar = getMax (currentCar);
		spawnCar(currentCar);		
	}

	public void buy(){
		if(!haveBus(currentCar)){
			if(mainController.saveController.data.money >= mainController.buses[currentCar].price){
				if(googleAnalytics){
					googleAnalytics.LogEvent(new EventHitBuilder()
					                         .SetEventCategory("Magazin")
					                         .SetEventAction("Procurare autobus")
					                         .SetEventLabel("Model")
					                         .SetEventValue(currentCar));
				}
				mainController.saveController.data.ownBuses.Add(currentCar);
				mainController.saveController.data.money -= mainController.buses[currentCar].price;
				updateCash();
				mainController.saveController.Save();
				spawnCar(currentCar);
			}
		}
	}

	public void select(){
		if(haveBus(currentCar)){
			if(googleAnalytics){
				googleAnalytics.LogEvent(new EventHitBuilder()
				                         .SetEventCategory("Magazin")
				                         .SetEventAction("Alegere autobus")
				                         .SetEventLabel("Model")
				                         .SetEventValue(currentCar));
			}
			mainController.saveController.data.bus = currentCar;
			mainController.saveController.Save();
			selectButton.SetActive(false);
		}
	}
}
