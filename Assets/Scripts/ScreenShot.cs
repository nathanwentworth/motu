using UnityEngine;
using System.Collections;
using System.IO;

public class ScreenShot : MonoBehaviour {

	void LateUpdate() {
		if(Input.GetButton("Fire1")){
			Application.CaptureScreenshot("Assets\\Resources\\Screenshot.png");
			Debug.Log ("*Camera Sound*");
		}
	}
}
