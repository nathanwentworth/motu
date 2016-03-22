using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class MaterialReassign : MonoBehaviour {

	private string SDPath;
	private string other;
    private string SI;
	public Manager gameManager;

	void Start(){
		SDPath = "file://" + Application.dataPath +"/Resources/StretchDog.png";
		other = "file://" + Application.dataPath +"/Resources/Other.png";
        SI = "file://" + Application.dataPath + "/Resources/Placeholder_Image.jpg";
        StartCoroutine("LoadImageStart");
    }

	void Update(){
		if (Input.GetButton ("HUD") && gameManager.picOfStretchDog) {
			StartCoroutine ("LoadImageStretchDog");
			//StartCoroutine ("LoadImageOther");
		}
	}

	IEnumerator LoadImageStretchDog(){
		Texture2D image = new Texture2D (2, 2);
		WWW www = new WWW(SDPath);
		yield return www;
		www.LoadImageIntoTexture (image);
		gameObject.GetComponent<RawImage> ().texture = image;
		yield return null;
	}

    IEnumerator LoadImageStart()
    {
        Texture2D image = new Texture2D(2, 2);
        WWW www = new WWW(SI);
        yield return www;
        www.LoadImageIntoTexture(image);
        gameObject.GetComponent<RawImage>().texture = image;
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
