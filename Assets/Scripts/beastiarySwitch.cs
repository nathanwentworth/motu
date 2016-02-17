using UnityEngine;
using System.Collections;

public class beastiarySwitch : MonoBehaviour {
	
	public GameObject[] animalPanels;
	public GameObject mainBeastUI;

	private bool mainUIOn;



	void Update() {
		if (Input.GetButtonDown("HUD") && mainUIOn == false) {
			mainBeastUI.SetActive(true);
			mainUIOn = true;
		}
		else if (Input.GetButtonDown("HUD") && mainUIOn == true) {	
			mainBeastUI.SetActive(false);
			mainUIOn = false;
		}
	}
	void Start() {
		for (int i = 0; i < animalPanels.Length; i++) {
			animalPanels[i].SetActive(false);
			if (i == 1)
				animalPanels[i].SetActive(true);
		}
	}

	public void menuSwitch(int menuPos) {
		animalPanels[menuPos].SetActive(true);
		for (int i = 0; i < animalPanels.Length; i++) {
			if (i != menuPos)
				animalPanels[i].SetActive(false);
		}
	}
}
