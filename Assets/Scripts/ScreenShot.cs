using UnityEngine;
using System.Collections;
using System.IO;

public class ScreenShot : MonoBehaviour {
	//Needs Input
	public GameObject ui_CameraUI;
	//Private Fields
	private bool startResetCameraUI;
	private float resetCameraUI;
	public bool aimDown;
	//Non-Input but Referenced
	public string whatAnimal;
	private AudioSource audioo;
	private float audioTime;
	public Manager gameManager;

	void Start(){
		resetCameraUI = 0.0f;
		ui_CameraUI.SetActive(false);
		audioo = gameObject.GetComponent<AudioSource>();
		audioo.volume = 0;
	}

	void Update(){
		if (gameManager.beatiaryOn == false) {
			if (Input.GetButton ("Fire2")) {
				aimDown = true;
				ui_CameraUI.SetActive (true);
			} else {
				aimDown = false;
				ui_CameraUI.SetActive (false);
			}
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

			//Reset the Camera UI that was disabled earlier
			if (startResetCameraUI) {
				resetCameraUI += Time.deltaTime;
			}

			if (resetCameraUI >= 0.1f) {
				ui_CameraUI.SetActive (true);
				startResetCameraUI = false;
				resetCameraUI = 0.0f;
			}
			/*audioTime += Time.deltaTime;
			if (audioTime > 10.5)
				audioo.volume += (Time.deltaTime * 0.05f);*/
		}
	}

	void Raycasting(){
		RaycastHit hit;
		Ray camRay = new Ray(transform.position, transform.forward);
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
