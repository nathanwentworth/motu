using UnityEngine;
using System.Collections;
using System.IO;

public class ScreenShot : MonoBehaviour {

	public GameObject ui_CameraUI;
	private bool startResetCameraUI;
	private float resetCameraUI;
	private bool camOn;

	private static int photoCount;

	void Start(){
		resetCameraUI = 0.0f;
		ui_CameraUI.SetActive(false);
		camOn = false;
		// Camera.aspect;
	}

	void Update(){
		if (Input.GetButtonDown("Fire2") && camOn == false) {
			ui_CameraUI.SetActive(true);
			camOn = true;
		}
		else if (Input.GetButtonDown("Fire2") && camOn == true) {
			ui_CameraUI.SetActive(false);
			camOn = false;
		}
		if (Input.GetButtonDown ("Fire1") && camOn == true) {
			ui_CameraUI.SetActive(false);
		}
	}

	void LateUpdate() {
		if (startResetCameraUI) {
			resetCameraUI += Time.deltaTime;
		}

		if (resetCameraUI >= 0.000001f) {
			ui_CameraUI.SetActive (true);
			startResetCameraUI = false;
			resetCameraUI = 0.0f;
		}
		
		if(Input.GetButtonDown("Fire1") && camOn == true){
			if (Application.isEditor) {
				Application.CaptureScreenshot ("Assets\\Resources\\Screenshot_" + photoCount + ".png");
			} else {
				Application.CaptureScreenshot ("Resources\\Screenshot_" + photoCount + ".png");
			}
			Debug.Log ("*Camera Sound*");
			startResetCameraUI = true;
			photoCount++;
		}
	}
}
