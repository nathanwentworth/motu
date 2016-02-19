using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {

	public bool enableInput;
	public bool beatiaryOn;
	public GameObject main_container;
	public LockMouse mouse;
	public ScreenShot screenshot;
	public GameObject pauseBG;

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
	
		if  (Input.GetKeyDown(KeyCode.Escape) && !beatiaryOn && !screenshot.aimDown){
			if (Time.timeScale == 0.0f) {
				Time.timeScale = 1.0f;
				mouse.Lock ();
				pauseBG.SetActive (false);
			} else {
				Time.timeScale = 0.0f;
				mouse.Unlock();
				pauseBG.SetActive (true);
			}
		}

		if (beatiaryOn == true) {
			mouse.Unlock ();
		}
	}
}
