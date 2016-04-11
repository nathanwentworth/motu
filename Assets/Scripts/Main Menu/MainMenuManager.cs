using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public Text LoadingText;
    public Slider ProgressBar;
    public GameObject LoadingContainer;
    public GameObject OptionsContainer;
    public GameObject MainContainer;
    private AsyncOperation sync;
    private bool startAnimation;

    public Slider options_VolumeSlider;
    public Dropdown options_ResolutionDrop;
    public Dropdown options_FullOrWindDrop;
    public Dropdown options_ControlSchemeDrop;

    private string volumeKey = "VOLUME_VALUE";
    private string resolutionKey = "RESOLUTION_VALUE";
    private string fullscreenKey = "FULLSCREEN_VALUE";
    private string controlSchemeKey = "CONTROLSCHEME_KEY";

    public float timeBetween;
    

	void Start () {
        options_VolumeSlider.value = PlayerPrefs.GetFloat(volumeKey, 0.75f);
        options_FullOrWindDrop.value = PlayerPrefs.GetInt(fullscreenKey, 0);
        options_ResolutionDrop.value = PlayerPrefs.GetInt(resolutionKey, 0);
        options_ControlSchemeDrop.value = PlayerPrefs.GetInt(controlSchemeKey, 0);

        ProgressBar.value = 0;
        Time.timeScale = 1;
        LoadingContainer.SetActive(false);
        OptionsContainer.SetActive(false);
        MainContainer.SetActive(true);

        if(options_FullOrWindDrop.value == 0)
        {
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
        }

	}

    void Update()
    {
        if (startAnimation)
        {
            ProgressBar.value = Mathf.Lerp(ProgressBar.value, sync.progress + 0.1f, timeBetween);
        }

        if(ProgressBar.value >= 0.95f && sync.progress == 0.9f)
        {
            sync.allowSceneActivation = true;
        }
    }

    public void FreePlayButton()
    {
        StartCoroutine(LoadingScreen("Test"));
    }

    public void ScavHuntButton()
    {
        StartCoroutine(LoadingScreen("scav"));
    }

    public void TutorialButton()
    {

    }

    public void GalleryButton()
    {
        StartCoroutine(LoadingScreen("GalleryTest"));
    }

    public void OptionsButton()
    {
        MainContainer.SetActive(false);
        OptionsContainer.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void Options_Save()
    {
        PlayerPrefs.SetInt(fullscreenKey, options_FullOrWindDrop.value);
        PlayerPrefs.SetInt(controlSchemeKey, options_ControlSchemeDrop.value);
        PlayerPrefs.SetInt(resolutionKey, options_ResolutionDrop.value);
        PlayerPrefs.SetFloat(volumeKey, options_VolumeSlider.value);

        if (options_FullOrWindDrop.value == 0)
        {
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
        }

        MainContainer.SetActive(true);
        OptionsContainer.SetActive(false);
    }

    IEnumerator LoadingScreen(string whatScene)
    {
        LoadingContainer.SetActive(true);
        sync =  SceneManager.LoadSceneAsync(whatScene, LoadSceneMode.Single);
        sync.allowSceneActivation = false;
        startAnimation = true;
        while (sync.progress < 0.9f)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

}
