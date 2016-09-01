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
    public GameObject CreditsPanel;
    private AsyncOperation sync;
    private bool startAnimation;

    public Slider options_VolumeSlider;
    public Dropdown options_ResolutionDrop;
    public Dropdown options_FullOrWindDrop;
    public Slider options_MouseSensitivity;
    public Text options_VolumeSliderValue;
    public Text options_MouseSliderValue;
    public GameObject soundManager;

    private string VOLUMEKEY = "VOLUME_VALUE";
    private string RESOLUTIONKEY = "RESOLUTION_VALUE";
    private string FULLSCREENKEY = "FULLSCREEN_VALUE";
    private string MOUSESENSITIVITYKEY = "MOUSESENSITIVITY_KEY";

    public float timeBetween;

    private bool wasResolutionChanged = false;
    private bool wasFullscreenChanged = false;
    private bool creditsDisplayed;

	public AudioMixerSnapshot END;
	

    void Start()
    {
        if (!MusicManager.Instance.Cpio.isPlaying)
        {
            MusicManager.Instance.MainMenuMusic();
        }
        DontDestroyOnLoad(soundManager);
        if (System.IO.Directory.Exists(Application.persistentDataPath + "/Photos/") != true)
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Photos/");
        }

        ProgressBar.value = 0;
        Time.timeScale = 1;
        LoadingContainer.SetActive(false);
        OptionsContainer.SetActive(false);
        MainContainer.SetActive(true);
        LockMouse.Unlock();

        // hide the credits panel on start
        creditsDisplayed = false;
        CreditsPanel.GetComponent<CanvasGroup>().alpha = 0;

        options_ResolutionDrop.options.Clear();
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            options_ResolutionDrop.options.Add(new Dropdown.OptionData(Screen.resolutions[i].ToString()));
        }
        options_VolumeSlider.value = PlayerPrefs.GetFloat(VOLUMEKEY, 0.7f);
        options_FullOrWindDrop.value = PlayerPrefs.GetInt(FULLSCREENKEY, 0);
        options_ResolutionDrop.value = PlayerPrefs.GetInt(RESOLUTIONKEY, 0);
        options_MouseSensitivity.value = PlayerPrefs.GetFloat(MOUSESENSITIVITYKEY, 2.5f);

        options_ResolutionDrop.RefreshShownValue();

        if (options_FullOrWindDrop.value == 0)
        {
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
        }

        MusicManager.Instance.SetMusicVolume(options_VolumeSlider.value);
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
        MusicManager.Instance.PlayConfirm();
        END.TransitionTo (5);
        StartCoroutine(LoadingScreen("Test"));
    }

    public void TutorialButton()
    {
        MusicManager.Instance.PlayConfirm();
        END.TransitionTo (5);
        StartCoroutine(LoadingScreen("tutorial"));
    }

    public void GalleryButton()
    {
        MusicManager.Instance.PlayConfirm();
        StartCoroutine(LoadingScreen("GalleryTest"));
    }

    public void OptionsButton()
    {
        MusicManager.Instance.PlayConfirm();
        MainContainer.SetActive(false);
        OptionsContainer.SetActive(true);
    }

    public void CreditsDisplay()
    {
        if (creditsDisplayed) {
            creditsDisplayed = false;
            CreditsPanel.GetComponent<CanvasGroup>().alpha = 0;
        } else {
            creditsDisplayed = true;
            CreditsPanel.GetComponent<CanvasGroup>().alpha = 1;
        }
        // StartCoroutine(FadeScreen());
    }

    public void QuitButton()
    {
        MusicManager.Instance.PlayDeny();
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

    // IEnumerator FadeScreen() {
    //     float start;
    //     float end;
    //     float elapsed = 0;
    //     if (creditsDisplayed) {
    //         start = 0f;
    //         end = 1f;
    //     } else {
    //         start = 1f;
    //         end = 0f;
    //     }
    //     while (elapsed < 1) {
    //         elapsed += Time.deltaTime;
    //         print(elapsed);
    //         CreditsPanel.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(start, end, Time.deltaTime);
    //     }
    //     yield return null;
    // }

}
