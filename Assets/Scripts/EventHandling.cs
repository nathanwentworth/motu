using UnityEngine;
using System.Collections;

public class EventHandling : MonoBehaviour {

	public Manager gameManager;

	void OnTriggerEnter(Collider collider){
		if (collider.tag == "MemoryCard1") {
			gameManager.nearMemCard1 = true;
			gameManager.MemCard1Object = collider.gameObject;
		}

		if (collider.tag == "StretchDog") {
			gameManager.notification_UI.SetActive (true);
			gameManager.instructions2 = true;
		}

		if (collider.tag == "TempHut") {
            if (gameManager.instructions4)
            {
                gameManager.notification_UI.SetActive(true);
            }
			gameManager.changeToForestLPF = true;
		}
	}

	void OnTriggerExit(Collider collider){
		if (collider.tag == "MemoryCard1") {
			gameManager.nearMemCard1 = false;
		}
		if (collider.tag == "TempHut") {
			gameManager.changeToForest = true;
		}
	}
}
