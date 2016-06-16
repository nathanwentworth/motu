using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;


public class GalleryManager : MonoBehaviour
{

    [Header("GameObject References")]
    public GameObject PhotoPrefab;
    public GameObject LoadingPanel;
    public GameObject PagePrefab;
    public GameObject LightBox;
    public GameObject LightBoxPhoto;
    public GameObject BackButton;
    public GameObject NextButton;
    public GameObject DeleteButton;
    public GameObject SaveButton;
    public GameObject ViewButton;

    public GameObject panel_ZoomScreen;
    public GameObject img_LargeFormat;
    public GameObject img_NextImage;
    public GameObject img_PrevImage;
    public Text txt_PhotoDate;

    public Text PhotosTaken;
    public Text TripsCompleted;
    public Text LoadingText;
    public Slider ProgressBar;
    [Header("Currently Selected Photo")]
    public int currentlySelectedPhoto = 0;
    //Private fields
    private List<GameObject> PagesList = new List<GameObject>();
    private List<GameObject> PhotosList = new List<GameObject>();
    private FileInfo[] allFiles;
    private int numberOfPages = 0;
    private int activePage = 1;
    private bool startProgressBar;
    private string dateTime;
    private string[] dateTimeArr;
    private char[] dateTimeSplit = {'_', '-'};
    private int currentlyViewedPhoto;
    private string photoMode;


    void Start()
    {
        Time.timeScale = 1;
        PhotosArray();
        if (allFiles.Length > 0)
        {
            StartCoroutine(CreateImages());
        }
    }

    void Update()
    {
        PhotosTaken.text = string.Format("{0} PHOTOS TAKEN", NumberOfPhotos());

        if (startProgressBar)
        {
            ProgressBar.value = Mathf.Lerp(ProgressBar.value, 1, 0.02f);
        }

        if(ProgressBar.value >= 0.95f && PhotosList.Count / NumberOfPhotos() == 1)
        {
            startProgressBar = false;
            LoadingPanel.SetActive(false);
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

    public void CloseZoom() {
      panel_ZoomScreen.SetActive(false);
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
        }
        PhotosArray();
        Debug.Log("Deleted photo " + currentlySelectedPhoto.ToString() + ".");
        currentlySelectedPhoto = 0;
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
        Debug.Log("Saving photo....");
        if (UnityEngine.Application.platform == RuntimePlatform.WindowsPlayer || UnityEngine.Application.platform == RuntimePlatform.WindowsEditor)
        {
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

    public void NextPrev(int indexChange) {
      if (currentlyViewedPhoto > 0 || currentlyViewedPhoto < allFiles.Length) {
        currentlyViewedPhoto = currentlyViewedPhoto + indexChange;
      }
      if (currentlyViewedPhoto <= 1) currentlyViewedPhoto = 1;
      if (currentlyViewedPhoto > allFiles.Length) currentlyViewedPhoto = allFiles.Length;
      ViewPhoto(currentlyViewedPhoto);
    }

    public void ViewPhoto(int photoNumber)
    {
      currentlyViewedPhoto = photoNumber;
      dateTime = allFiles[photoNumber-1].ToString();
      dateTime = dateTime.Replace(UnityEngine.Application.persistentDataPath.ToString() + "/Photos/TitleHere_", "");
      dateTime = dateTime.Replace(".png", "");
      dateTimeArr = dateTime.Split(dateTimeSplit);
      txt_PhotoDate.text = "PHOTO TAKEN ON:\n" + dateTimeArr[1] + " / " + dateTimeArr[2] + " / " + dateTimeArr[3] + " at " + dateTimeArr[4] + ":" + dateTimeArr[5] + ":" + dateTimeArr[6];
      if (dateTimeArr[0] == "0") photoMode = "TAKEN IN:\nFREE MODE";
      else photoMode = "TAKEN IN:\nSCAVENGER HUNT";
      img_LargeFormat.GetComponent<RawImage>().texture = GameObject.Find("Photo " + photoNumber).GetComponent<RawImage>().texture;
      if ((photoNumber + 1) <= allFiles.Length) {
        img_NextImage.GetComponent<RawImage>().texture = GameObject.Find("Photo " + (photoNumber + 1)).GetComponent<RawImage>().texture;      
      } else {
        img_NextImage.GetComponent<RawImage>().texture = null;
      }
      if ((photoNumber - 1) > 0) {
        img_PrevImage.GetComponent<RawImage>().texture = GameObject.Find("Photo " + (photoNumber - 1)).GetComponent<RawImage>().texture;        
      } else {
        img_PrevImage.GetComponent<RawImage>().texture = null;
      }
      print("bloop " + photoNumber);
      panel_ZoomScreen.SetActive(true);
    }

    IEnumerator CreateImages()
    {
        LoadingPanel.SetActive(true);
        startProgressBar = true;
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
            photo.transform.SetParent(PagePrefab.transform);
            photo.GetComponent<RawImage>().texture = image;
            photo.transform.localPosition = Vector3.zero;
            photo.GetComponent<PictureSelect>().photoNumber = photoNumber + 1;
            photo.name = "Photo " + (photoNumber + 1).ToString();
            photo.transform.localScale = Vector3.one;
            PhotosList.Add(photo);
        }
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
