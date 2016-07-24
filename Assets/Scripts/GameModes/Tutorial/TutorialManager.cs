using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class TutorialManager : MonoBehaviour
{

    public MovementObjective objective1;
    public Text tutText;
    public GameObject doge;
    private int tutIndex;
    private bool objTrigger1;
    private bool objTrigger2;
    private float resWidth = 0;
    private float resHeight = 0;
    private int currentGameMode = 1;
    public GameObject secondaryCam;
    private Camera photoCamera;
    public GameObject mainCam;
    private Camera viewCamera;
    public int zoomSensitivity;
    public int zoomMin;
    public int zoomMax;
    private string TUTORIALCOMPLETE = "TUTORIAL_COMPLETE";
    private string TRIPSTAKEN = "TRIPS_TAKEN";

    private string[] tutArr1 = { "Welcome to OuterWorld Image!", "In this game, you will be taking pictures on an alien planet!", "Use the mouse to look around.",
        "Use WASD to move around.", "Press Spacebar to jump.", "Press Left Shift or the Y button to crouch.", "When you're ready, go to the objective point on the next island!" };
    private string[] tutArr2 = { "Great job! To use your camera, use the left mouse button.", "To zoom in, use the mouse wheel.", "Why don't you practice by taking a picture of that strange looking dog over there!" };
    private string[] tutArr3 = { "Nice Shot! Now that you know the basics, you can play the full game by going to Free Play in the Main Menu.", "To view the photos you have taken, you can go to Gallery in the Main Menu.", "Now Loading the Main Menu. Have fun and enjoy!" };

    public AudioMixerSnapshot DEFAULT;

    void Start()
    {
        DEFAULT.TransitionTo(3);
        MusicManager.Instance.StopAllMusic();
        MusicManager.Instance.StartCoroutine(MusicManager.Instance.Playlist());
        LockMouse.Lock();
        Time.timeScale = 1;
        tutIndex = 0;
        TutorialTextDisp();
        resWidth = Screen.currentResolution.width;
        resHeight = Screen.currentResolution.height;
        photoCamera = secondaryCam.GetComponent<Camera>();
        viewCamera = mainCam.GetComponent<Camera>();
    }

    void TutorialTextDisp()
    {

        if (objTrigger1 == false && tutIndex <= tutArr1.Length - 1)
        {
            tutText.text = tutArr1[tutIndex];
            StartCoroutine(Timer(5));
        }
        else if (objTrigger1 && !objTrigger2 && tutIndex <= tutArr2.Length - 1)
        {
            tutText.text = tutArr2[tutIndex];
            StartCoroutine(Timer(5));
        }
        else if (objTrigger2 && objTrigger1 && tutIndex <= tutArr3.Length - 1)
        {

            tutText.text = tutArr3[tutIndex];
            StartCoroutine(Timer(6));
        }
    }

    void FixedUpdate()
    {
        float zoomValue = Input.GetAxis("Mouse ScrollWheel");
        viewCamera.fieldOfView = Mathf.Clamp(viewCamera.fieldOfView + (-zoomValue * zoomSensitivity), zoomMin, zoomMax);
        photoCamera.fieldOfView = viewCamera.fieldOfView;
    }

    void Update()
    {
        if (objective1.MovementObjectiveTriggered)
        {
            MusicManager.Instance.PlayConfirm();
            tutIndex = 0;
            doge.SetActive(true);
            objTrigger1 = true;
            StopAllCoroutines();
            TutorialTextDisp();
            objective1.MovementObjectiveTriggered = false;
        }

        if(objTrigger1 && objTrigger2 && tutIndex == tutArr3.Length - 1)
        {
            StartCoroutine(Timer(5));
        }
        else if(objTrigger1 && objTrigger2 && tutIndex == tutArr3.Length)
        {
            PlayerPrefs.SetInt(TUTORIALCOMPLETE, 1);
            PlayerPrefs.SetInt(TRIPSTAKEN, PlayerPrefs.GetInt(TRIPSTAKEN) + 1);
            MusicManager.Instance.StopPlayList();
            MusicManager.Instance.StopAllMusic();
            SceneManager.LoadScene("MainMenuTest");
        }
        if (Input.GetButtonDown("Cancel")) {
            MusicManager.Instance.StopPlayList();
            MusicManager.Instance.StopAllMusic();
            SceneManager.LoadScene("MainMenuTest");
        }


        if (Input.GetButtonDown("Fire1"))
        {
            //Gets the MusicManager script, fires off the sound for taking a photo
            MusicManager.Instance.PlayCameraClick();
            RenderTexture rt = new RenderTexture((int)resWidth, (int)resHeight, 24);
            photoCamera.targetTexture = rt;
            Texture2D screenShot = new Texture2D((int)resWidth, (int)resHeight, TextureFormat.RGB24, false);
            photoCamera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            photoCamera.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(currentGameMode);
            System.IO.File.WriteAllBytes(filename, bytes);

            RaycastHit hit;
            Ray hitRay = new Ray(mainCam.transform.position, mainCam.transform.forward);
            if (Physics.Raycast(hitRay, out hit))
            {
                if(hit.collider.tag == "StretchDog")
                {
                    MusicManager.Instance.PlayConfirm();
                    tutIndex = 0;
                    objTrigger2 = true;
                    StopAllCoroutines();
                    TutorialTextDisp();
                }
            }
        }
    }

    public static string ScreenShotName(int currentGameMode)
    {
        return string.Format("{0}/OuterWorld_{1}_{2}.png", Application.persistentDataPath + "/Photos", currentGameMode, DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        tutIndex++;
        TutorialTextDisp();
    }
}
