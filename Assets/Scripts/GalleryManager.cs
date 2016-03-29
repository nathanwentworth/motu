using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GalleryManager : MonoBehaviour {

	public GameObject photoTemplate;
	public GameObject canvas;
    public GameObject panel;

    private FileInfo[] allFiles;
    List<GameObject> Page = new List<GameObject>();
    private int numberOfPages = -1;
    private int activePage = 0;


    void Start(){
        PhotosArray();
        CreateNewPage();
        for (int i = 0; i < NumberOfPhotos(); i++) {
            StartCoroutine (CreateImages (i));
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

    public void CreateNewPage()
    {
        numberOfPages++;
        Debug.Log(string.Format("Creating panel number {0}.", numberOfPages));
        GameObject pageClone = Instantiate(panel) as GameObject;
        Page.Add(pageClone);
        Page[numberOfPages].transform.SetParent(canvas.transform, false);
        Page[numberOfPages].transform.localPosition = Vector3.zero;
    }

    public void NextPage()
    {
        if(activePage != numberOfPages)
        {
            Page[activePage].SetActive(false);
            activePage++;
            Page[activePage].SetActive(true);
        }
    }

    public void LastPage()
    {
        if(activePage != 0)
        {
            Page[activePage].SetActive(false);
            activePage--;
            Page[activePage].SetActive(true);
        }
    }

    IEnumerator CreateImages(int photoNumber)
	{
        //Upload the file into the game
		Texture2D image = new Texture2D(2, 2);
        WWW www = new WWW("file://" + allFiles[photoNumber].ToString());
        yield return www;
        //Load it as a texture
        www.LoadImageIntoTexture(image);
        //Create photo in game
        GameObject photo = Instantiate (photoTemplate);
        Debug.Log(string.Format("Creating photo {0}.", photoNumber));
        //Put it in the proper page
        if(photoNumber / 9 == numberOfPages + 1)
        {
            CreateNewPage();
            Page[numberOfPages].SetActive(false);
        }
        photo.transform.SetParent (Page[numberOfPages].transform, false);
        Debug.Log(string.Format("Adding photo {0} to panel {1}", photoNumber, numberOfPages));
        //Set the texture to the photo
        photo.GetComponent<RawImage>().texture = image;
		photo.transform.localPosition = Vector3.zero;
		yield return null;
	}
}
