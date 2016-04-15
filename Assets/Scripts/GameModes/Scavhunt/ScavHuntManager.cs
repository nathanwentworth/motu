using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class ScavHuntManager : MonoBehaviour
{

    public bool enableMovement = true;

    [Header("Public Variables")]
    public int zoomSensitivity;
    public int zoomMin;
    public int zoomMax;

    [Header("GameObject References")]
    public GameObject pauseBG;
    //public GameObject ui_CameraUI;
    public GameObject mainCam;
    public GameObject secondaryCam;

    //Private Fields
    //private bool aimDown = false;
    //private bool isAnotherUIActive = false;
    private Camera viewCamera;
    private float resWidth = 0;
    private float resHeight = 0;
    private Camera photoCamera;
    private int currentGameMode = 0;
    private bool isAnotherUIActive = false;
    private bool isPaused = false;

    //ScavHuntStuff
    private static System.Random rng = new System.Random();
    List<scavItems> items = new List<scavItems>();

    public Text instructions;
    public Text timerText;
    public GameObject instructionsPanel;
    public AnimationCurve fade;

    private float menuAnimTimer = 0;
    private float timer = 600;
    private int timerInt;
    private int huntedCount;
    private float min;
    private float sec;
    private bool fadePanel;

    public GameObject gameOverContainer;
    public Text gameOverText;

    void Start()
    {
        if (System.IO.Directory.Exists(Application.persistentDataPath + "/Photos/") != true)
        {
            Debug.Log("Creating Photos Directory");
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Photos/");
        }

        viewCamera = mainCam.GetComponent<Camera>();
        photoCamera = secondaryCam.GetComponent<Camera>();
        LockMouse.Unlock();
        Time.timeScale = 0;
        huntedCount = 0;

        items.Add(new scavItems("Cube_1", "First Cube"));
        items.Add(new scavItems("Cube_2", "Second Cube"));
        items.Add(new scavItems("Cube_3", "Third Cube"));
        items.Add(new scavItems("Cube_4", "Fourth Cube"));
        items.Add(new scavItems("Cube_5", "Fifth Cube"));
        items.Add(new scavItems("Cube_6", "Sixth Cube"));

        int n = items.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            scavItems value = items[k];
            items[k] = items[n];
            items[n] = value;
        }

        for (int i = 0; i < 3; i++)
        {
            print(items[i].nameReadable);
        }
        for (int i = items.Count - 1; i >= 3; i--)
        {
            items.RemoveAt(i);
        }
        gameOverContainer.SetActive(false);
        instructionsPanel.SetActive(true);
        instructions.text = "FIND THESE THINGS: \n" + items[0].nameReadable.ToUpper() + ",\n" + items[1].nameReadable.ToUpper() + ",\n" + items[2].nameReadable.ToUpper();

        AudioListener.pause = false;
        //ui_CameraUI.SetActive(false);
        resWidth = Screen.currentResolution.width;
        resHeight = Screen.currentResolution.height;
    }

    void FixedUpdate()
    {
        if (enableMovement)
        {
            float zoomValue = Input.GetAxis("Mouse ScrollWheel");
            viewCamera.fieldOfView = Mathf.Clamp(viewCamera.fieldOfView + (-zoomValue * zoomSensitivity), zoomMin, zoomMax);
            photoCamera.fieldOfView = viewCamera.fieldOfView;
        }

        //Look for changes in screen resolution
        resWidth = Screen.currentResolution.width;
        resHeight = Screen.currentResolution.height;

        if (Input.GetKeyDown(KeyCode.Escape) && !isAnotherUIActive && !isPaused)
        {
            Pause();
        }
        //Unpause game
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            unPause();
        }
        //Take a picture 
        if (Input.GetButtonDown("Fire1") && !isPaused && enableMovement)
        {
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
            Debug.Log(string.Format("Took picture to: {0}", filename));

            RaycastHit hit;
            Ray hitRay = new Ray(mainCam.transform.position, mainCam.transform.forward);
            if (Physics.Raycast(hitRay, out hit))
            {
                for (int i = items.Count - 1; i >= 0; i--)
                {
                    if (hit.transform.gameObject.name == items[i].nameItem)
                    {
                        print("cool dude, you hit " + items[i].nameReadable);
                        items.RemoveAt(i);
                        huntedCount++;
                    }
                }
            }
        }
    }

    void Update()
    {

        if (Time.timeScale == 1)
        {
            timer -= Time.unscaledDeltaTime;
            timer = Mathf.Clamp(timer, 0, 600);
            timerInt = (int)timer;
            sec = timerInt % 60;
            min = timerInt / 60;
            if (sec > 9)
            {
                timerText.text = min + ":" + sec;
            }
            else
            {
                timerText.text = min + ":0" + sec;
            }
        }

        if (timer <= 0 || huntedCount == 3)
        {
            LockMouse.Unlock();
            Time.timeScale = 0;
            enableMovement = false;
            gameOverContainer.SetActive(true);
            if (timer <= 0)
            {
                gameOverText.text = string.Format("Time up!\nYou found {0} out of 3 objects!", huntedCount);
            }
            else
            {
                gameOverText.text = "Nice job!\nYou found all 3 objects!";
            }
        }

        if (fadePanel)
        {
            LockMouse.Lock();
            Time.timeScale = 1;
            menuAnimTimer += Time.unscaledDeltaTime;
            instructionsPanel.GetComponent<CanvasGroup>().alpha = fade.Evaluate(menuAnimTimer);
            if (menuAnimTimer >= 1)
                fadePanel = false;
        }

    }

    public static string ScreenShotName(int currentGameMode)
    {
        return string.Format("{0}/TitleHere_{1}_{2}.png", Application.persistentDataPath + "/Photos", currentGameMode, DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    private void Pause()
    {
        isAnotherUIActive = true;
        isPaused = true;
        Time.timeScale = 0.0f;
        LockMouse.Unlock();
        pauseBG.SetActive(true);
        AudioListener.pause = true;
        enableMovement = false;
    }

    public void unPause()
    {
        isPaused = false;
        isAnotherUIActive = false;
        Time.timeScale = 1.0f;
        LockMouse.Lock();
        pauseBG.SetActive(false);
        AudioListener.pause = false;
        enableMovement = true;
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenuTest");
    }

    public void FadePanel()
    {
        fadePanel = true;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("scav");
    }
}
