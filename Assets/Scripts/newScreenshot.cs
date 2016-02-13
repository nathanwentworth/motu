using UnityEngine;
using System.Collections;
using System.IO;

public class newScreenshot : MonoBehaviour {

	private static int photoCount;


	void Update() {
		if (Input.GetButtonDown ("Fire1")) {
			UploadPNG();
		}		
	}


	IEnumerator UploadPNG() {
		// We should only read the screen buffer after rendering is complete
		yield return new WaitForEndOfFrame();

		// Create a texture the size of the screen, RGB24 format
		int width = (int)Screen.width;
		int height = (int)Screen.height;
		// int width = (int)Screen.width * 0.625;
		// int height = (int)Screen.height * 0.74;
		Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

		// Read screen contents into the texture
		// tex.ReadPixels(new Rect(Screen.width * 0.1875, Screen.height * 0.13, width, height), 0, 0);
		tex.ReadPixels(new Rect(Screen.width, Screen.height, width, height), 0, 0);
		tex.Apply();

		// Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG();
		Object.Destroy(tex);

		// For testing purposes, also write to a file in the project folder
		File.WriteAllBytes(Application.dataPath + "Assets\\Resources\\Screenshot_" + photoCount + ".png", bytes);

		photoCount++;
	}


}
