using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public Text LoadingText;
    public GameObject LoadingContainer;

	void Start () {
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
		LoadingContainer.SetActive(true);
        AsyncOperation sync =  SceneManager.LoadSceneAsync(whatScene, LoadSceneMode.Single);
        sync.allowSceneActivation = false;
		while (true)
        {
            LoadingText.text = "Loading";
            yield return new WaitForSeconds(0.25f);
            LoadingText.text = "Loading.";
            yield return new WaitForSeconds(0.25f);
            LoadingText.text = "Loading..";
            yield return new WaitForSeconds(0.25f);
            LoadingText.text = "Loading...";
            yield return new WaitForSeconds(0.25f);
			if (sync.isDone) {
				Debug.Log("Done Loading, padding time.");
				yield return new WaitForSeconds (3);
			}
        }
        yield return null;
    }
}
