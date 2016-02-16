using UnityEngine;
using System.Collections;
using System.IO;

public class ScreenShot : MonoBehaviour {
	//Needs Input
	public GameObject ui_CameraUI;
	//Private Fields
	private bool startResetCameraUI;
	private float resetCameraUI;
	//Non-Input but Referenced
	public string whatAnimal;

	void Start(){
		resetCameraUI = 0.0f;
	}

	void Update(){
		if (Input.GetButtonDown ("Fire1")) {
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
			} 
			else {
				if (Application.isEditor) {
					Application.CaptureScreenshot ("Assets\\Resources\\Other.png");
				} 
				else {
					Application.CaptureScreenshot ("Resources\\Other.png");
				}

				startResetCameraUI = true;
			}
		}

		//Reset the Camera UI that was disabled earlier
		if (startResetCameraUI) {
			resetCameraUI += Time.deltaTime;
		}

		if (resetCameraUI >= 0.000001f) {
			ui_CameraUI.SetActive (true);
			startResetCameraUI = false;
			resetCameraUI = 0.0f;
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
