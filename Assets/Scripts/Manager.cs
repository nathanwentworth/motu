using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {

	public bool enableInput;
	public GameObject main_container;

	void Start(){
		enableInput = true;
	}

	void Update(){
		if (main_container.activeSelf == true) {
			print ("fhfh");
			enableInput = false;
		} else {
			enableInput = true;
		}
	}
}
