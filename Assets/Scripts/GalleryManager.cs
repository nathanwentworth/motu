﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;


public class GalleryManager : MonoBehaviour {

	public GameObject photoTemplate;
	public GameObject canvas;
    public GameObject panel;

    private FileInfo[] allFiles;
    List<GameObject> Page = new List<GameObject>();
    List<GameObject> Photos = new List<GameObject>();
    private int numberOfPages = -1;
    private int activePage = 0;
    public int currentPhotoHighlighted = -1;


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
		totalFiles = Directory.GetFiles(UnityEngine.Application.persistentDataPath + "/Photos/", "TitleHere*.png").Length;
		return totalFiles;
	}

   public void PhotosArray()
    {
        DirectoryInfo di = new DirectoryInfo(UnityEngine.Application.persistentDataPath + "/Photos/");
        allFiles = di.GetFiles();
    }

    //public void DeleteAllPhotos()
    //{
    //    Directory.Delete(Application.persistentDataPath + "/Photos/", true);
    //    Directory.CreateDirectory(Application.persistentDataPath + "/Photos/");
    //    Debug.Log("All photos deleted.");
    //    SceneManager.LoadScene("GalleryTest");
    //}

    public void CreateNewPage()
    {
        numberOfPages++;
        Debug.Log(string.Format("Creating panel number {0}.", numberOfPages));
        GameObject pageClone = Instantiate(panel) as GameObject;
        Page.Add(pageClone);
        Page[numberOfPages].transform.SetParent(canvas.transform, false);
        Page[numberOfPages].transform.localPosition = new Vector3(-25, 0, 0);
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

    public void DeletePhoto()
    {
        Debug.Log("Deleted photo " + currentPhotoHighlighted.ToString() + ".");
        File.Delete(allFiles[currentPhotoHighlighted].ToString());
        Destroy(Photos[currentPhotoHighlighted]);
    }

    public void SavePhoto()
    {
        Debug.Log("Saving photo....");
        SaveFileDialog save = new SaveFileDialog();
        save.Filter = "PNG Image|*.png";
        save.Title = "Save your photo!";
        save.ShowDialog();
        StartCoroutine(Save(currentPhotoHighlighted, save.FileName));
    }

    public void ViewPhoto()
    {
        Debug.Log("Enlarging photo....");
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
        photo.GetComponent<PictureSelect>().photoNumber = photoNumber;
        Photos.Add(photo);
		yield return null;
	}

    IEnumerator Save(int photoNumber, string path)
    {
        Texture2D image = new Texture2D(2, 2);
        WWW www = new WWW("file://" + allFiles[photoNumber].ToString());
        yield return www;
        //Load it as a texture
        www.LoadImageIntoTexture(image);
        byte[] bytes = image.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
        yield return null;
    }
}