using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public Text LoadingText;
    public Slider ProgressBar;
    public GameObject LoadingContainer;
    private AsyncOperation sync;
    private bool startAnimation;

    public float timeBetween;
    

	void Start () {
        ProgressBar.value = 0;
        Time.timeScale = 1;
        LoadingContainer.SetActive(false);
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

    }

    public void QuitButton()
    {
        Application.Quit();
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
