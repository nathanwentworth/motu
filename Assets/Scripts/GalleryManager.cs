using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GalleryManager : MonoBehaviour {

	private int numPhotos = 0;
	public GameObject photoTemplate;
	public GameObject canvas;

	void Start(){
		numPhotos = 1;
		for (int i = 0; i < numPhotos; i++) {
			StartCoroutine (LoadImage (i));
			}
		}

	public static int NumberOfPhotos()
	{
		int totalFiles = 0;
		totalFiles = System.IO.Directory.GetFiles(Application.persistentDataPath + "/Photos/", "MOTU*.png").Length;
		return totalFiles;
	}

	IEnumerator LoadImage(int photoNumber)
	{
		Texture2D image = new Texture2D(2, 2);
		print (string.Format ("{0}/MOTUv2_0_{1}.png", Application.persistentDataPath + "/Photos/", photoNumber));
		WWW www = new WWW(string.Format("file://{0}/MOTUv2_0_{1}.png", Application.persistentDataPath + "/Photos/", photoNumber));
		yield return www;
		www.LoadImageIntoTexture(image);
		GameObject photo = Instantiate (photoTemplate);
		photo.transform.SetParent (canvas.transform);
		photo.GetComponent<RawImage>().texture = image;
		photo.transform.localPosition = Vector3.zero;
		yield return null;
	}
}
