using UnityEngine;
using System.Collections;
using System.IO;

public class MaterialReassign : MonoBehaviour {

	private string url;

	void Start(){
		url = "file://" + Application.dataPath +"/Resources/Screenshot.png";
		print (url);
	}

	void Update(){
		if (Input.GetButton ("Fire1")) {
			StartCoroutine ("LoadImage");
		}
	}

	IEnumerator LoadImage(){
		Texture2D image = new Texture2D (2, 2);
		WWW www = new WWW(url);
		yield return www;
		www.LoadImageIntoTexture (image);
		gameObject.GetComponent<Renderer> ().material.mainTexture = image;
		yield return null;
	}
}
