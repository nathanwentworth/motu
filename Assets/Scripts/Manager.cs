using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {

	public bool enableInput;
	public GameObject main_container;
	public bool beatiaryOn;
	public LockMouse mouse;

	void Start(){
		enableInput = true;
		beatiaryOn = false;
		mouse.Lock ();
	}

	void Update(){
		if (main_container.activeSelf == true) {
			enableInput = false;
			beatiaryOn = true;
		} else {
			enableInput = true;
			beatiaryOn = false;
		}
		if( Input.GetMouseButtonDown(0) && Time.timeScale > 0.0f && beatiaryOn == false) {
			mouse.Lock ();
		}
		if  (Input.GetKeyDown(KeyCode.Escape) || beatiaryOn == true )
		{
			mouse.Unlock ();
		}
	}
}
