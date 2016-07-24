using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
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

    public Slider options_VolumeSlider;
    public Dropdown options_ResolutionDrop;
    public Dropdown options_FullOrWindDrop;
    public Slider options_MouseSensitivity;
    public Text options_VolumeSliderValue;
    public Text options_MouseSliderValue;
    public GameObject OptionsContainer;
    public GameObject MainContainer;
    private string VOLUMEKEY = "VOLUME_VALUE";
    private string RESOLUTIONKEY = "RESOLUTION_VALUE";
    private string FULLSCREENKEY = "FULLSCREEN_VALUE";
    private string MOUSESENSITIVITYKEY = "MOUSESENSITIVITY_KEY";
    private string TRIPSTAKEN = "TRIPS_TAKEN";
    private bool wasResolutionChanged = false;
    private bool wasFullscreenChanged = false;

	public AudioMixerSnapshot DEFAULT;

    void Start()
    {
		DEFAULT.TransitionTo (3);
        MusicManager.Instance.StopAllMusic();
        MusicManager.Instance.StartCoroutine(MusicManager.Instance.Playlist());
        isPaused = false;
        isAnotherUIActive = false;
        Time.timeScale = 1.0f;
        LockMouse.Lock();
        pauseBG.SetActive(false);
        AudioListener.pause = false;
        if (System.IO.Directory.Exists (Application.persistentDataPath + "/Photos/") != true) {
			System.IO.Directory.CreateDirectory (Application.persistentDataPath + "/Photos/");
		}
        viewCamera = mainCam.GetComponent<Camera>();
        photoCamera = secondaryCam.GetComponent<Camera>();
        //ui_CameraUI.SetActive(false);
        resWidth = Screen.currentResolution.width;
        resHeight = Screen.currentResolution.height;

        options_ResolutionDrop.options.Clear();
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            options_ResolutionDrop.options.Add(new Dropdown.OptionData(Screen.resolutions[i].ToString()));
        }
        options_VolumeSlider.value = PlayerPrefs.GetFloat(VOLUMEKEY, 0.75f);
        options_FullOrWindDrop.value = PlayerPrefs.GetInt(FULLSCREENKEY, 0);
        options_ResolutionDrop.value = PlayerPrefs.GetInt(RESOLUTIONKEY, 0);
        options_MouseSensitivity.value = PlayerPrefs.GetFloat(MOUSESENSITIVITYKEY, 2.5f);

        options_ResolutionDrop.RefreshShownValue();
        PlayerPrefs.SetInt(TRIPSTAKEN, PlayerPrefs.GetInt(TRIPSTAKEN) + 1);
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
        // resWidth = Screen.currentResolution.width;
        // resHeight = Screen.currentResolution.height;
    }

    void Update()
    {
        // options_MouseSliderValue.text = string.Format("{0:F1}", options_MouseSensitivity.value);

        // options_VolumeSliderValue.text = string.Format("{0:F0}%", options_VolumeSlider.value * 100);
        //INPUTS
        //AimDown Camera
        //if (Input.GetButton("Fire2") && !isAnotherUIActive)
        //{
        //    aimDown = true;
        //    isAnotherUIActive = true;
        //    ui_CameraUI.SetActive(true);
        //}
        ////Un-aim Camera
        //else if (!Input.GetButton("Fire2") && aimDown)
        //{
        //    isAnotherUIActive = false;
        //    aimDown = false;
        //    ui_CameraUI.SetActive(false);
        //}
        //Pause the game
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
        if (Input.GetButtonDown("Fire1") && !isPaused)
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
			string filename = ScreenShotName (currentGameMode);
			System.IO.File.WriteAllBytes(filename, bytes);
        }
    }

    public static string ScreenShotName(int currentGameMode)
    {
		return string.Format("{0}/OuterWorld_{1}_{2}.png", Application.persistentDataPath + "/Photos", currentGameMode, DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    private void Pause()
    {
        MusicManager.Instance.PlayConfirm();
        isAnotherUIActive = true;
        isPaused = true;
        Time.timeScale = 0.0f;
        LockMouse.Unlock();
        pauseBG.SetActive(true);
        //AudioListener.pause = true;
    }

    public void unPause()
    {
        MusicManager.Instance.PlayConfirm();
        isPaused = false;
        isAnotherUIActive = false;
        Time.timeScale = 1.0f;
        LockMouse.Lock();
        pauseBG.SetActive(false);
        AudioListener.pause = false;
    }

    public void OptionsButton()
    {
        MusicManager.Instance.PlayConfirm();
        MainContainer.SetActive(false);
        OptionsContainer.SetActive(true);
    }

    public void ExitToMainMenu()
    {
        MusicManager.Instance.PlayDeny();
        MusicManager.Instance.StopPlayList();
        MusicManager.Instance.StopAllMusic();
        SceneManager.LoadScene("MainMenuTest");
    }

    public void Quit()
    {
		//END.TransitionTo (1);
        Application.Quit();
    }

    public void ResolutionChanged()
    {
        wasResolutionChanged = true;
    }

    public void FullscreenChanged()
    {
        wasFullscreenChanged = true;
    }

    public void Options_Save()
    {
        MusicManager.Instance.PlayConfirm();
        PlayerPrefs.SetInt(FULLSCREENKEY, options_FullOrWindDrop.value);
        PlayerPrefs.SetFloat(MOUSESENSITIVITYKEY, options_MouseSensitivity.value);
        PlayerPrefs.SetInt(RESOLUTIONKEY, options_ResolutionDrop.value);
        PlayerPrefs.SetFloat(VOLUMEKEY, options_VolumeSlider.value);

        bool fullscreen = Screen.fullScreen;

        if (wasFullscreenChanged)
        {

            if (options_FullOrWindDrop.value == 0)
            {
                Screen.fullScreen = true;
                fullscreen = true;

            }
            else
            {
                Screen.fullScreen = false;
                fullscreen = false;
            }
        }

        if (wasResolutionChanged)
        {
            Screen.SetResolution(Screen.resolutions[options_ResolutionDrop.value].width, Screen.resolutions[options_ResolutionDrop.value].height, fullscreen, Screen.resolutions[options_ResolutionDrop.value].refreshRate);
        }

        MusicManager.Instance.SetMusicVolume(options_VolumeSlider.value);

        MainContainer.SetActive(true);
        OptionsContainer.SetActive(false);
        wasFullscreenChanged = false;
        wasResolutionChanged = false;
    }
}

