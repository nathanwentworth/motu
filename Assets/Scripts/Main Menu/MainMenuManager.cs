using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MainMenuManager : MonoBehaviour
{

    public Text LoadingText;
    public Slider ProgressBar;
    public GameObject LoadingContainer;
    public GameObject OptionsContainer;
    public GameObject MainContainer;
    public GameObject FreePlayButtonGO;
    public GameObject FreePlayButtonText;
    public GameObject GalleryButtonGO;
    public GameObject GalleryButtonText;
    private AsyncOperation sync;
    private bool startAnimation;

    public Slider options_VolumeSlider;
    public Dropdown options_ResolutionDrop;
    public Dropdown options_FullOrWindDrop;
    public Slider options_MouseSensitivity;
    public Text options_VolumeSliderValue;
    public Text options_MouseSliderValue;

    private string VOLUMEKEY = "VOLUME_VALUE";
    private string RESOLUTIONKEY = "RESOLUTION_VALUE";
    private string FULLSCREENKEY = "FULLSCREEN_VALUE";
    private string MOUSESENSITIVITYKEY = "MOUSESENSITIVITY_KEY";
    private string TUTORIALCOMPLETE = "TUTORIAL_COMPLETE";

    public float timeBetween;

    private bool wasResolutionChanged = false;
    private bool wasFullscreenChanged = false;
    private int tutorialComplete = 0;

	[Header("AudioSources")]
	public AudioSource Confirm;
	public AudioSource Deny;
	public AudioMixerSnapshot END;
	

    void Start()
    {
        if (System.IO.Directory.Exists(Application.persistentDataPath + "/Photos/") != true)
        {
            Debug.Log("Creating Photos Directory");
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Photos/");
        }

        ProgressBar.value = 0;
        Time.timeScale = 1;
        LoadingContainer.SetActive(false);
        OptionsContainer.SetActive(false);
        MainContainer.SetActive(true);
        LockMouse.Unlock();

        options_ResolutionDrop.options.Clear();
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            options_ResolutionDrop.options.Add(new Dropdown.OptionData(Screen.resolutions[i].ToString()));
        }
        options_VolumeSlider.value = PlayerPrefs.GetFloat(VOLUMEKEY, 0.75f);
        options_FullOrWindDrop.value = PlayerPrefs.GetInt(FULLSCREENKEY, 0);
        options_ResolutionDrop.value = PlayerPrefs.GetInt(RESOLUTIONKEY, 0);
        options_MouseSensitivity.value = PlayerPrefs.GetFloat(MOUSESENSITIVITYKEY, 2.5f);
        tutorialComplete = PlayerPrefs.GetInt(TUTORIALCOMPLETE, 0);

        options_ResolutionDrop.RefreshShownValue();

        if (options_FullOrWindDrop.value == 0)
        {
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
        }

        if(tutorialComplete == 1)
        {
            FreePlayButtonGO.GetComponent<Button>().interactable = true;
            FreePlayButtonText.GetComponent<Button>().interactable = true;
            GalleryButtonGO.GetComponent<Button>().interactable = true;
            GalleryButtonText.GetComponent<Button>().interactable = true;
        }
        else
        {
            FreePlayButtonGO.GetComponent<Button>().interactable = false;
            FreePlayButtonText.GetComponent<Button>().interactable = false;
            GalleryButtonGO.GetComponent<Button>().interactable = false;
            GalleryButtonText.GetComponent<Button>().interactable = false;
        }
    }

    void Update()
    {
        if (startAnimation)
        {
            ProgressBar.value = Mathf.Lerp(ProgressBar.value, sync.progress + 0.1f, timeBetween);
        }

        if (ProgressBar.value >= 0.95f && sync.progress == 0.9f)
        {
            sync.allowSceneActivation = true;
        }

        // options_MouseSliderValue.text = string.Format("{0:F1}", options_MouseSensitivity.value);

        // options_VolumeSliderValue.text = string.Format("{0:F0}%", options_VolumeSlider.value * 100);
    }

    public void FreePlayButton()
    {
		Confirm.Play ();
		END.TransitionTo (5);
        StartCoroutine(LoadingScreen("Test"));
    }

    public void TutorialButton()
    {
		Confirm.Play ();
		END.TransitionTo (5);
        StartCoroutine(LoadingScreen("tutorial"));
    }

    public void GalleryButton()
    {
		Confirm.Play ();
        StartCoroutine(LoadingScreen("GalleryTest"));
    }

    public void OptionsButton()
    {
		Confirm.Play ();
        MainContainer.SetActive(false);
        OptionsContainer.SetActive(true);
    }

    public void QuitButton()
    {
		Deny.Play ();
		END.TransitionTo (1);
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
		Confirm.Play ();
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
            Debug.Log(Screen.resolutions[options_ResolutionDrop.value]);
        }

        Debug.Log(Screen.fullScreen);

        MainContainer.SetActive(true);
        OptionsContainer.SetActive(false);
        wasFullscreenChanged = false;
        wasResolutionChanged = false;
    }

    IEnumerator LoadingScreen(string whatScene)
    {
        LoadingContainer.SetActive(true);
        sync = SceneManager.LoadSceneAsync(whatScene, LoadSceneMode.Single);
        sync.allowSceneActivation = false;
        startAnimation = true;
        while (sync.progress < 0.9f)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

}
