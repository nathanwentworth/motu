using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;


public class GalleryManager : MonoBehaviour {

	public GameObject photoTemplate;
	public GameObject canvas;
    public GameObject panel;
    public GameObject LightBox;
    public GameObject LightBoxPhoto;
    public GameObject DeleteButton;
    public GameObject SaveButton;
    public GameObject ViewButton;

    private FileInfo[] allFiles;
    List<GameObject> Page = new List<GameObject>();
    List<GameObject> Photos = new List<GameObject>();
    private int numberOfPages = -1;
    private int activePage = 0;
    public int currentPhotoHighlighted = -1;
    private bool somethingIsSelected;

    void Start(){
        PhotosArray();
        CreateNewPage();
        for (int i = 0; i < NumberOfPhotos(); i++) {
            StartCoroutine (CreateImages (i));
		}
	}

    void Update()
    {
        if(currentPhotoHighlighted < 0)
        {
            DeleteButton.SetActive(false);
            SaveButton.SetActive(false);
            ViewButton.SetActive(false);
        }
        else
        {
            DeleteButton.SetActive(true);
            SaveButton.SetActive(true);
            ViewButton.SetActive(true);
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

    public void CreateNewPage()
    {
        numberOfPages++;
        Debug.Log(string.Format("Creating panel number {0}.", numberOfPages));
        GameObject pageClone = Instantiate(panel) as GameObject;
        Page.Add(pageClone);
        Page[numberOfPages].transform.SetParent(canvas.transform, false);
        Page[numberOfPages].transform.localPosition = new Vector3(-25, 0, 0);
        Page[numberOfPages].name = "Page" + numberOfPages.ToString();
    }

    public void NextPage()
    {
        currentPhotoHighlighted = -1;
        if(activePage != numberOfPages)
        {
            Page[activePage].SetActive(false);
            activePage++;
            Page[activePage].SetActive(true);
        }
    }

    public void ClickOffSomething()
    {
        currentPhotoHighlighted = -1;
        Debug.Log("Clicked on nothing.");
    }

    public void LastPage()
    {
        currentPhotoHighlighted = -1;
        if (activePage != 0)
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
        Photos.RemoveAt(currentPhotoHighlighted);
        for (int i = 0; i < Photos.Count; i++)
        {
            Photos[i].GetComponent<PictureSelect>().photoNumber = i;
            Photos[i].name = "photo" + i.ToString();
        }
        for (int i = activePage; i < numberOfPages; i++)
        {
            int temp = (i + 1) * 8;
            Photos[temp].transform.SetParent(null);
            Photos[temp].transform.SetParent(Page[i].transform);
        }
        currentPhotoHighlighted = -1;
    }

    public void SavePhoto()
    {
        Debug.Log("Saving photo....");
        if (UnityEngine.Application.platform == RuntimePlatform.WindowsPlayer || UnityEngine.Application.platform == RuntimePlatform.WindowsEditor) {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "PNG Image|*.png";
            save.Title = "Save your photo!";
            save.ShowDialog();
            StartCoroutine(Save(currentPhotoHighlighted, save.FileName));
        }
        else 
        {
            string filename = Path.GetFileName(allFiles[currentPhotoHighlighted].ToString());
            StartCoroutine(Save(currentPhotoHighlighted, UnityEngine.Application.dataPath + "/" + filename));
        }
    }

    public void ViewPhoto()
    {
        StartCoroutine(CreateLightBoxImage(currentPhotoHighlighted));
    }

    public void CloseLightbox()
    {
        LightBox.SetActive(false);
    }

    IEnumerator CreateLightBoxImage(int photoNumber)
    {
        //Upload the file into the game
        Texture2D image = new Texture2D(2, 2);
        WWW www = new WWW("file://" + allFiles[photoNumber].ToString());
        yield return www;
        //Load it as a texture
        www.LoadImageIntoTexture(image);
        //Set the texture to the photo
        LightBoxPhoto.GetComponent<RawImage>().texture = image;
        yield return LightBoxPhoto;
        LightBox.SetActive(true);
        yield return null;
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
        photo.name = "photo" + photoNumber.ToString();
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
        Debug.Log("Saving photo to " + path);
        yield return null;
    }
}
