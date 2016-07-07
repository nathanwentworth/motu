using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UnityEngine.SceneManagement;

public class GalleryManager : MonoBehaviour
{

    [Header("GameObject References")]
    public GameObject PhotoPrefab;
    public GameObject LoadingPanel;
    public GameObject PhotoContainer;
    public GameObject panel_Main;
    public GameObject panel_ZoomScreen;
    public GameObject img_LargeFormat;
    public GameObject img_NextImage;
    public GameObject img_PrevImage;
    public Text PhotoCounter;
    public Text txt_PhotoMode;
    public Text txt_PhotoDate;
    public GameObject panel_noPhoto;

    public Text PhotosTaken;
    public Text TripsCompleted;
    [Header("Currently Selected Photo")]
    //Private fields
    private List<GameObject> PagesList = new List<GameObject>();
    private List<GameObject> PhotosList = new List<GameObject>();
    private FileInfo[] allFiles;
    private int numberOfPages = 0;
    private int activePage = 1;
    private string dateTime;
    private string[] dateTimeArr;
    private char[] dateTimeSplit = { '_', '-' };
    private int currentlyViewedPhoto;
    private string photoMode;
    private string zoomPhotoCounter;


    void Start()
    {
        Time.timeScale = 1;
        PhotosArray();
        if (allFiles.Length > 0)
        {
            StartCoroutine(CreateImages());
            panel_noPhoto.SetActive(false);
        } else {
          panel_noPhoto.SetActive(true);
        }
    }

    void Update()
    {
        //
        PhotosTaken.text = string.Format("{0} PHOTOS TAKEN", NumberOfPhotos());

        if (PhotosList.Count / NumberOfPhotos() == 1)
        {
            LoadingPanel.SetActive(false);
        }
    }

    public void ClickedNothing()
    {
        currentlyViewedPhoto = 0;
        Debug.Log("Clicked on nothing.");
    }

    public void CloseZoom()
    {
        panel_ZoomScreen.SetActive(false);
        panel_Main.GetComponent<CanvasGroup>().alpha = 1;
        panel_Main.GetComponent<CanvasGroup>().interactable = true;
    }

    public void DeletePhoto()
    {
        File.Delete(allFiles[currentlyViewedPhoto - 1].ToString());
        Destroy(PhotosList[currentlyViewedPhoto - 1]);
        PhotosList.RemoveAt(currentlyViewedPhoto - 1);
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
        }
        PhotosArray();
        Debug.Log("Deleted photo " + currentlyViewedPhoto.ToString() + ".");
        currentlyViewedPhoto = 0;
        CloseZoom();
        if(NumberOfPhotos() == 0)
        {
            panel_noPhoto.SetActive(true);
        }
    }

    public static float NumberOfPhotos()
    {
        int totalFiles = 0;
        totalFiles = Directory.GetFiles(UnityEngine.Application.persistentDataPath + "/Photos/", "TitleHere*.png").Length;
        return totalFiles;
    }

    public void PhotosArray()
    {
        DirectoryInfo di = new DirectoryInfo(UnityEngine.Application.persistentDataPath + "/Photos/");
        allFiles = di.GetFiles("TitleHere*.png");
    }

    public void SavePhoto()
    {
        Debug.Log("Saving photo...." + currentlyViewedPhoto);
        if (UnityEngine.Application.platform == RuntimePlatform.WindowsPlayer || UnityEngine.Application.platform == RuntimePlatform.WindowsEditor)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "PNG Image|*.png";
            save.Title = "Select where to save your photo!";
            save.ShowDialog();
            StartCoroutine(Save(currentlyViewedPhoto, save.FileName));
        }
        else
        {
            string filename = Path.GetFileName(allFiles[currentlyViewedPhoto].ToString());
            StartCoroutine(Save(currentlyViewedPhoto, UnityEngine.Application.dataPath + "/" + filename));
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenuTest");
    }

    public void NextPrev(int indexChange)
    {
        if (currentlyViewedPhoto > 0 || currentlyViewedPhoto < allFiles.Length)
        {
            currentlyViewedPhoto = currentlyViewedPhoto + indexChange;
        }
        if (currentlyViewedPhoto <= 1) currentlyViewedPhoto = 1;
        if (currentlyViewedPhoto > allFiles.Length) currentlyViewedPhoto = allFiles.Length;
        ViewPhoto(currentlyViewedPhoto);
    }

    public void ViewPhoto(int photoNumber)
    {
        currentlyViewedPhoto = photoNumber;
        dateTime = allFiles[photoNumber - 1].ToString();
        dateTime = dateTime.Replace(UnityEngine.Application.persistentDataPath.ToString() + "/Photos/TitleHere_", "");
        dateTime = dateTime.Replace(".png", "");
        dateTimeArr = dateTime.Split(dateTimeSplit);
        txt_PhotoDate.text = "PHOTO TAKEN ON:\n" + dateTimeArr[1] + " / " + dateTimeArr[2] + " / " + dateTimeArr[3] + " at " + dateTimeArr[4] + ":" + dateTimeArr[5] + ":" + dateTimeArr[6];
        if (dateTimeArr[0] == "0") photoMode = "TAKEN IN:\nFREE MODE";
        else photoMode = "TAKEN IN:\nTUTORIAL";
        txt_PhotoMode.text = photoMode;
        zoomPhotoCounter = currentlyViewedPhoto + "/" + allFiles.Length;
        PhotoCounter.text = zoomPhotoCounter;
        img_LargeFormat.GetComponent<RawImage>().texture = GameObject.Find("Photo " + photoNumber).GetComponent<RawImage>().texture;
        if ((photoNumber + 1) <= allFiles.Length)
        {
            img_NextImage.GetComponent<RawImage>().texture = GameObject.Find("Photo " + (photoNumber + 1)).GetComponent<RawImage>().texture;
        }
        else {
            img_NextImage.GetComponent<RawImage>().texture = null;
        }
        if ((photoNumber - 1) > 0)
        {
            img_PrevImage.GetComponent<RawImage>().texture = GameObject.Find("Photo " + (photoNumber - 1)).GetComponent<RawImage>().texture;
        }
        else {
            img_PrevImage.GetComponent<RawImage>().texture = null;
        }
        panel_ZoomScreen.SetActive(true);
        panel_Main.GetComponent<CanvasGroup>().alpha = 0;
        panel_Main.GetComponent<CanvasGroup>().interactable = false;
    }

    IEnumerator CreateImages()
    {
        LoadingPanel.SetActive(true);
        for (int photoNumber = 0; photoNumber < NumberOfPhotos(); photoNumber++)
        {
            //Upload the file into the game
            Texture2D image = new Texture2D(2, 2);
            WWW www = new WWW("file://" + allFiles[photoNumber].ToString());
            yield return www;
            //Load it as a texture
            www.LoadImageIntoTexture(image);
            //Create photo in game
            GameObject photo = Instantiate(PhotoPrefab);
            Debug.Log(string.Format("Creating photo {0}.", photoNumber + 1));
            //Set the texture to the photo
            photo.transform.SetParent(PhotoContainer.transform);
            photo.GetComponent<RawImage>().texture = image;
            photo.transform.localPosition = Vector3.zero;
            photo.GetComponent<PictureSelect>().photoNumber = photoNumber + 1;
            photo.name = "Photo " + (photoNumber + 1).ToString();
            photo.transform.localScale = Vector3.one;
            PhotosList.Add(photo);
        }
        yield return null;
    }

    IEnumerator Save(int photoNumber, string path)
    {
        Texture2D image = new Texture2D(2, 2);
        Debug.Log(allFiles[photoNumber - 1]);
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
