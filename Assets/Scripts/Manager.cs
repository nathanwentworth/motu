using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

	[Header("Enable Input")]
	public bool enableMovement;

	[Header("GameObject References")]
	public GameObject pauseBG;
	public Text Notif_TXT;
	private bool isAnotherUIActive;
	public GameObject mainBeastUI;
	public GameObject ui_CameraUI;
	public GameObject mainCam;
	public GameObject[] animalPanels;

	//Private Fields
	LockMouse mouse = new LockMouse ();
	private bool startResetCameraUI;
	private bool mainUIOn;
	private float resetCameraUI;
	private bool aimDown;

	//Non-Input but Referenced
	[Header("Animal Photographed")]
	public string whatAnimal;

	void Start(){

		//Give Variables Properties
		mainBeastUI.SetActive (false);
		for (int i = 0; i < animalPanels.Length; i++) {
			animalPanels[i].SetActive(false);
			if (i == 0)
				animalPanels[i].SetActive(true);
		}
		enableMovement = true;
		mouse.Lock ();
		AudioListener.pause = false;
		isAnotherUIActive = false;
		aimDown = false;
		ui_CameraUI.SetActive (false);
	}

	void Update(){
	
	//INPUTS
		//AimDown Camera
		if (Input.GetButton ("Fire2") && !isAnotherUIActive) {
			aimDown = true;
			isAnotherUIActive = true;
			ui_CameraUI.SetActive (true);
		} 
		//Un-aim Camera
		else if(!Input.GetButton ("Fire2") && aimDown) {
			isAnotherUIActive = false;
			aimDown = false;
			ui_CameraUI.SetActive (false);
		}
		//Open Beastiary
		if (Input.GetButtonDown("HUD") && mainUIOn == false && aimDown == false && !isAnotherUIActive) {
			mainBeastUI.SetActive(true);
			mainUIOn = true;
			isAnotherUIActive = true;
			mouse.Unlock ();
			enableMovement = false;
		}
		//Close Beastiary
		else if (Input.GetButtonDown("HUD") && mainUIOn == true) {
			isAnotherUIActive = false;	
			mainBeastUI.SetActive(false);
			isAnotherUIActive = false;
			mainUIOn = false;
			mouse.Lock ();
			enableMovement = true;
		}
		//Pause the game
		if  (Input.GetKeyDown(KeyCode.Escape) && !isAnotherUIActive && Time.timeScale == 1.0f){
			isAnotherUIActive = true;
			Time.timeScale = 0.0f;
			mouse.Unlock();
			pauseBG.SetActive (true);
			AudioListener.pause = true;
		}
		//Unpause game
		else if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0.0f) {
			isAnotherUIActive = false;
			Time.timeScale = 1.0f;
			mouse.Lock ();
			pauseBG.SetActive (false);
			AudioListener.pause = false;
		}
		//Take a picture but only if the camera is aimed down
		if (Input.GetButtonDown ("Fire1") && aimDown) {
			//Disable Camera UI
			ui_CameraUI.SetActive (false);
			//Send out a ray that returns a string
			Raycasting ();

			//Take the screenshot and save it depending on what was hit in the raycast
			if (whatAnimal == "stretchdog") {
				if (Application.isEditor) {
					Application.CaptureScreenshot ("Assets\\Resources\\StretchDog.png");
				} else {
					Application.CaptureScreenshot ("Resources\\StretchDog.png");
				}

				startResetCameraUI = true;
			} else {
				if (Application.isEditor) {
					Application.CaptureScreenshot ("Assets\\Resources\\Other.png");
				} else {
					Application.CaptureScreenshot ("Resources\\Other.png");
				}

				startResetCameraUI = true;
			}
		}
	
	//OTHER CONDITIONAL STATEMENTS
		//Start the camera reset timer
		if (startResetCameraUI) {
			resetCameraUI += Time.deltaTime;
		}
		//Reset the camera
		if (resetCameraUI >= 0.1f) {
			ui_CameraUI.SetActive (true);
			startResetCameraUI = false;
			resetCameraUI = 0.0f;
		}

		if (Notif_TXT.IsActive()) {
			Notif_TXT.text = "Press <color=green>W</color> <color=green>A</color> <color=green>S</color> <color=green>D</color> to move.";
		}
	}

	//Switch Beastiary Menu on Button Press
	public void menuSwitch(int menuPos) {
		animalPanels[menuPos].SetActive(true);
		for (int i = 0; i < animalPanels.Length; i++) {
			if (i != menuPos)
				animalPanels[i].SetActive(false);
		}
	}

	//Send out a ray to see what animal were taking a pic of
	void Raycasting(){
		RaycastHit hit;
		Ray camRay = new Ray(mainCam.transform.position, mainCam.transform.forward);
		if (Physics.Raycast (camRay, out hit, 25)) {
			if (hit.collider.tag == "StretchDog") {
				whatAnimal = "stretchdog";
				Debug.Log (hit.distance);
			} 
			else {
				whatAnimal = "other";
			}
		}
	}
}
