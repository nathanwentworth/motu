using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;


public class GalleryManager : MonoBehaviour {

    [Header("GameObject References")]
	public GameObject PhotoPrefab;
	public GameObject Container;
    public GameObject PagePrefab;
    public GameObject LightBox;
    public GameObject LightBoxPhoto;
    public GameObject BackButton;
    public GameObject NextButton;
    public GameObject DeleteButton;
    public GameObject SaveButton;
    public GameObject ViewButton;
    [Header("Currently Selected Photo")]
    public int currentlySelectedPhoto = 0;
    //Private fields
    private List<GameObject> PagesList = new List<GameObject>();
    private List<GameObject> PhotosList = new List<GameObject>();
    private FileInfo[] allFiles;
    private int numberOfPages = 0;
    private int activePage = 1;

    void Start(){
        PhotosArray();
        if (allFiles.Length > 0)
        {
            CreateNewPage();
            for (int i = 0; i < NumberOfPhotos(); i++)
            {
                StartCoroutine(CreateImages(i));
            }
        }
    }

    void Update()
    {
        if(currentlySelectedPhoto < 1)
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
        if (activePage == 1 || numberOfPages == 0)
        {
            BackButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
        else
        {
            BackButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        if (activePage == numberOfPages || numberOfPages == 0)
        {
            NextButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
        else
        {
            NextButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
    }

    public void Back()
    {
        currentlySelectedPhoto = 0;
        if (activePage != 1)
        {
            PagesList[activePage - 1].SetActive(false);
            activePage--;
            Debug.Log("Going to page " + activePage + ".");
            PagesList[activePage - 1].SetActive(true);
        }
    }

    public void ClickedNothing()
    {
        currentlySelectedPhoto = 0;
        Debug.Log("Clicked on nothing.");
    }

    public void CloseLightbox()
    {
        LightBox.SetActive(false);
    }

    public void CreateNewPage()
    {
        numberOfPages++;
        Debug.Log(string.Format("Creating page number {0}.", numberOfPages));
        GameObject pageClone = Instantiate(PagePrefab) as GameObject;
        PagesList.Add(pageClone);
        PagesList[numberOfPages - 1].transform.SetParent(Container.transform, false);
        PagesList[numberOfPages - 1].transform.localPosition = new Vector3(-25, 0, 0);
        PagesList[numberOfPages - 1].name = "Page " + numberOfPages.ToString();
    }

    public void DeletePhoto()
    {
        File.Delete(allFiles[currentlySelectedPhoto - 1].ToString());
        Destroy(PhotosList[currentlySelectedPhoto - 1]);
        PhotosList.RemoveAt(currentlySelectedPhoto - 1);
        for (int i = 0; i < PhotosList.Count; i++)
        {
            PhotosList[i].GetComponent<PictureSelect>().photoNumber = i + 1;
            PhotosList[i].name = "Photo " + (i + 1).ToString();
        }
        for (int i = activePage; i < numberOfPages; i++)
        {
            int temp = i * 9;
            PhotosList[temp - 1].transform.SetParent(null);
            PhotosList[temp - 1].transform.SetParent(PagesList[i - 1].transform);
        }
        if (PhotosList.Count == ((numberOfPages - 1) * 9))
        {
            Destroy(PagesList[numberOfPages - 1]);
            PagesList.RemoveAt(numberOfPages - 1);
            numberOfPages--;
            if(activePage > numberOfPages && numberOfPages != 0)
            {
                activePage--;
                Debug.Log("Going to page " + activePage + ".");
                PagesList[activePage - 1].SetActive(true);
            }
        }
        PhotosArray();
        Debug.Log("Deleted photo " + currentlySelectedPhoto.ToString() + ".");
        currentlySelectedPhoto = 0;
    }

    public void Next()
    {
        currentlySelectedPhoto = 0;
        if (activePage != numberOfPages)
        {
            PagesList[activePage - 1].SetActive(false);
            activePage++;
            Debug.Log("Going to page " + activePage + ".");
            PagesList[activePage - 1].SetActive(true);
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

    public void SavePhoto()
    {
        Debug.Log("Saving photo....");
        if (UnityEngine.Application.platform == RuntimePlatform.WindowsPlayer || UnityEngine.Application.platform == RuntimePlatform.WindowsEditor) {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "PNG Image|*.png";
            save.Title = "Select where to save your photo!";
            save.ShowDialog();
            StartCoroutine(Save(currentlySelectedPhoto, save.FileName));
        }
        else 
        {
            string filename = Path.GetFileName(allFiles[currentlySelectedPhoto].ToString());
            StartCoroutine(Save(currentlySelectedPhoto, UnityEngine.Application.dataPath + "/" + filename));
        }
    }

    public void ViewPhoto()
    {
        StartCoroutine(CreateLightBoxImage(currentlySelectedPhoto));
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
        GameObject photo = Instantiate (PhotoPrefab);
        Debug.Log(string.Format("Creating photo {0}.", photoNumber + 1));
        //Put it in the proper page
        if(photoNumber / 9 == numberOfPages)
        {
            CreateNewPage();
            PagesList[numberOfPages - 1].SetActive(false);
        }
        photo.transform.SetParent (PagesList[numberOfPages - 1].transform, false);
        Debug.Log(string.Format("Adding photo {0} to page {1}.", photoNumber + 1, numberOfPages));
        //Set the texture to the photo
        photo.GetComponent<RawImage>().texture = image;
		photo.transform.localPosition = Vector3.zero;
        photo.GetComponent<PictureSelect>().photoNumber = photoNumber + 1;
        photo.name = "Photo " + (photoNumber + 1).ToString();
        PhotosList.Add(photo);
		yield return null;
	}

    IEnumerator CreateLightBoxImage(int photoNumber)
    {
        //Upload the file into the game
        Texture2D image = new Texture2D(2, 2);
        WWW www = new WWW("file://" + allFiles[photoNumber - 1].ToString());
        yield return www;
        //Load it as a texture
        www.LoadImageIntoTexture(image);
        //Set the texture to the photo
        LightBoxPhoto.GetComponent<RawImage>().texture = image;
        yield return LightBoxPhoto;
        LightBox.SetActive(true);
        yield return null;
    }

    IEnumerator Save(int photoNumber, string path)
    {
        Texture2D image = new Texture2D(2, 2);
        WWW www = new WWW("file://" + allFiles[photoNumber - 1].ToString());
        yield return www;
        //Load it as a texture
        www.LoadImageIntoTexture(image);
        byte[] bytes = image.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
        Debug.Log("Saving photo to " + path);
        yield return null;
    }
}
