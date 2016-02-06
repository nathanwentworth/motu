using UnityEngine;
using System.Collections;
using System.IO;

public class ScreenShot : MonoBehaviour {

	public GameObject ui_CameraUI;
	private bool startResetCameraUI;
	private float resetCameraUI;

	void Start(){
		resetCameraUI = 0.0f;
	}

	void Update(){
		if (Input.GetButtonDown ("Fire1")) {
			ui_CameraUI.SetActive (false);
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
		
		if(Input.GetButtonDown("Fire1")){
			if (Application.isEditor) {
				Application.CaptureScreenshot ("Assets\\Resources\\Screenshot.png");
			} else {
				Application.CaptureScreenshot ("Resources\\Screenshot.png");
			}
			Debug.Log ("*Camera Sound*");
			startResetCameraUI = true;
		}
	}
}
