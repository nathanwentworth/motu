using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public Text LoadingText;
    public GameObject LoadingContainer;
    private AsyncOperation sync;

    public float timeBetween;
    

	void Start () {
        Time.timeScale = 1;
        LoadingContainer.SetActive(false);
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
        StartCoroutine(LoadingTextAnimation());
        LoadingContainer.SetActive(true);
        sync =  SceneManager.LoadSceneAsync(whatScene, LoadSceneMode.Single);
        sync.allowSceneActivation = false;
        while(sync.progress < 0.9f)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(3);
        sync.allowSceneActivation = true;
        yield return null;
    }

    IEnumerator LoadingTextAnimation()
    {
        while (true)
        {
            LoadingText.text = "Loading";
            yield return new WaitForSeconds(timeBetween);
            LoadingText.text = "Loading.";
            yield return new WaitForSeconds(timeBetween);
            LoadingText.text = "Loading..";
            yield return new WaitForSeconds(timeBetween);
            LoadingText.text = "Loading...";
            yield return new WaitForSeconds(timeBetween);
        }
    }
}
