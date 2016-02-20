using UnityEngine;
using System.Collections;

public class EventHandling : MonoBehaviour {

	public AudioSource MemCard1Audio;
	private bool playMemCard1;
	private GameObject MemCard1Object;

	void Start(){
		playMemCard1 = false;
	}

	void Update(){
		if (playMemCard1 && Input.GetButtonDown ("Submit")) {
			MemCard1Audio.Play ();
			MemCard1Object.SetActive (false);
			playMemCard1 = false;
		}
			
	}

	void OnTriggerEnter(Collider collider){
		if (collider.tag == "MemoryCard1") {
			playMemCard1 = true;
			MemCard1Object = collider.gameObject;
		}
	}

	void OnTriggerExit(Collider collider){
		if (collider.tag == "MemoryCard1") {
			playMemCard1 = false;
		}
	}
}
