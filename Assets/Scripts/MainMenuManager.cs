using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public Text LoadingText;
    public GameObject LoadingContainer;

	void Start () {
        StartCoroutine(LoadingTextChange());
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
        /*AsyncOperation sync =  SceneManager.LoadSceneAsync(whatScene, LoadSceneMode.Single);
        yield return sync;
        yield return new WaitForSeconds(3);*/
        yield return null;
    }

    IEnumerator LoadingTextChange()
    {
        Debug.Log("Starting Coroutine...");
        yield return new WaitForSeconds(1);
        Debug.Log("Waited one second");
        yield return null;
        /*while(true)
        {
            LoadingText.text = "Loading";
            yield return new WaitForSeconds(0.5f);
            LoadingText.text = "Loading.";
            yield return new WaitForSeconds(0.5f);
            LoadingText.text = "Loading..";
            yield return new WaitForSeconds(0.5f);
            LoadingText.text = "Loading...";
            yield return new WaitForSeconds(0.5f);
        }*/
    }
}
