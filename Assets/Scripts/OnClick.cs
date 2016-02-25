using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class OnClick : MonoBehaviour {

    void Start()
    {
        LockMouse mouse = new LockMouse();
        mouse.Unlock();
    }

    public void Clickerino()
    {
        SceneManager.LoadScene("Test");
    }
}
