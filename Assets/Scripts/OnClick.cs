using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class OnClick : MonoBehaviour {

	public AudioSource notification;

    void Start()
    {
        AudioListener.pause = false;
        LockMouse mouse = new LockMouse();
        mouse.Unlock();
    }

    public void Clickerino()
    {
		notification.Play ();
        SceneManager.LoadScene("Test");
    }

    public void Quit()
    {
        notification.Play();
        Application.Quit();
    }
}
