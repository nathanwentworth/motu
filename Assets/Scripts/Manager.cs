using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class Manager : MonoBehaviour {

	public bool enableInput;
	public bool beatiaryOn;
	public GameObject main_container;
	public ScreenShot screenshot;
	public GameObject pauseBG;
	LockMouse mouse = new LockMouse ();

	void Start(){
		enableInput = true;
		beatiaryOn = false;
		mouse.Lock ();
		AudioListener.pause = false;
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
				AudioListener.pause = false;
			} else {
				Time.timeScale = 0.0f;
				mouse.Unlock();
				pauseBG.SetActive (true);
				AudioListener.pause = true;
			}
		}

		if (beatiaryOn == true) {
			mouse.Unlock ();
		}
	}
}
