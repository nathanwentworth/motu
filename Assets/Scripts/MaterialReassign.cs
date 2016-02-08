using UnityEngine;
using System.Collections;
using System.IO;

public class MaterialReassign : MonoBehaviour {

	private string SDPath;
	private string other;
	public ScreenShot screenshot;

	void Start(){
		SDPath = "file://" + Application.dataPath +"/Resources/StretchDog.png";
		other = "file://" + Application.dataPath +"/Resources/Other.png";
	}

	void Update(){
		if (Input.GetButton ("Fire1")) {
			switch (screenshot.whatAnimal) {
			case "stretchdog":
				if (this.gameObject.tag == "StretchDogScreen") {
					StartCoroutine ("LoadImageStretchDog");
				}
				break;

			case "other":
				if (this.gameObject.tag == "OtherScreen") {
					StartCoroutine ("LoadImageOther");
				}
				break;
			}
		}
	}

	IEnumerator LoadImageStretchDog(){
		Texture2D image = new Texture2D (2, 2);
		WWW www = new WWW(SDPath);
		yield return www;
		www.LoadImageIntoTexture (image);
		gameObject.GetComponent<Renderer> ().material.mainTexture = image;
		yield return null;
	}

	IEnumerator LoadImageOther(){
		Texture2D image = new Texture2D (2, 2);
		WWW www = new WWW(other);
		yield return www;
		www.LoadImageIntoTexture (image);
		gameObject.GetComponent<Renderer> ().material.mainTexture = image;
		yield return null;
	}
}
