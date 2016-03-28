using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class GalleryManager : MonoBehaviour {

	public GameObject photoTemplate;
	public GameObject canvas;
    private FileInfo[] allFiles;


    void Start(){
        PhotosArray();
        for (int i = 0; i < NumberOfPhotos(); i++) {
            StartCoroutine (LoadImage (i));
		}
	}

	public static int NumberOfPhotos()
	{
		int totalFiles = 0;
		totalFiles = Directory.GetFiles(Application.persistentDataPath + "/Photos/", "MOTU*.png").Length;
		return totalFiles;
	}

   public void PhotosArray()
    {
        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/Photos/");
        allFiles = di.GetFiles();
    }

    public void DeleteAllPhotos()
    {
        Directory.Delete(Application.persistentDataPath + "/Photos/", true);
        Directory.CreateDirectory(Application.persistentDataPath + "/Photos/");
        Debug.Log("All photos deleted.");
        SceneManager.LoadScene("GalleryTest");
    }

    IEnumerator LoadImage(int photoNumber)
	{
		Texture2D image = new Texture2D(2, 2);
        WWW www = new WWW("file://" + allFiles[photoNumber].ToString());
        yield return www;
		www.LoadImageIntoTexture(image);
		GameObject photo = Instantiate (photoTemplate);
        Debug.Log(string.Format("Creating photo {0}.", photoNumber));
		photo.transform.SetParent (canvas.transform);
		photo.GetComponent<RawImage>().texture = image;
		photo.transform.localPosition = Vector3.zero;
		yield return null;
	}
}
