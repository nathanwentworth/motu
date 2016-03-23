﻿using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{

    [Header("Enable Movement")]
    public bool enableMovement = true;

    [Header("Public Variables")]
    public int resWidth = 2550;
    public int resHeight = 3300;

    [Header("GameObject References")]
    //public GameObject pauseBG;
    public GameObject ui_CameraUI;
    public GameObject secondaryCam;

    [Header("Audio References")]
    public AudioSource[] CameraClicks;

    //Private Fields
    private bool aimDown = false;
    private bool isAnotherUIActive = false;
    private Camera photoCamera;
    private int currentGameMode;

    void Start()
    {
        photoCamera = secondaryCam.GetComponent<Camera>();
        LockMouse.Lock();
        Time.timeScale = 1;
        AudioListener.pause = false;
        ui_CameraUI.SetActive(false);
    }

    void Update()
    {
        //INPUTS
        //AimDown Camera
        if (Input.GetButton("Fire2") && !isAnotherUIActive)
        {
            aimDown = true;
            isAnotherUIActive = true;
            ui_CameraUI.SetActive(true);
        }
        //Un-aim Camera
        else if (!Input.GetButton("Fire2") && aimDown)
        {
            isAnotherUIActive = false;
            aimDown = false;
            ui_CameraUI.SetActive(false);
        }
        //Pause the game
        //if (Input.GetKeyDown(KeyCode.Escape) && !isAnotherUIActive && Time.timeScale == 1.0f)
        //{
        //    isAnotherUIActive = true;
        //    Time.timeScale = 0.0f;
        //    mouse.Unlock();
        //    pauseBG.SetActive(true);
        //    AudioListener.pause = true;
        //}
        ////Unpause game
        //else if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0.0f)
        //{
        //    isAnotherUIActive = false;
        //    Time.timeScale = 1.0f;
        //    mouse.Lock();
        //    pauseBG.SetActive(false);
        //    AudioListener.pause = false;
        //}
        //Take a picture but only if the camera is aimed down
        if (Input.GetButtonDown("Fire1") && aimDown)
        {
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            photoCamera.targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            photoCamera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            photoCamera.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(currentGameMode, NumberOfPhotos());
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
            StartCoroutine(LoadImage(filename));
        }
    }

    public static string ScreenShotName(int currentGameMode, int photoNumber)
    {
        return string.Format("{0}/MOTUv2_{1}_{2}.png", Application.streamingAssetsPath, currentGameMode, photoNumber);
    }

    public static int NumberOfPhotos()
    {
        int totalFiles = 0;
        totalFiles = System.IO.Directory.GetFiles(Application.streamingAssetsPath, "MOTU*.png").Length;
        return totalFiles;
    }

    IEnumerator LoadImage(string fileName)
    {
        print("Loading image...");
        string filePath = string.Format("file://{0}", fileName);
        Texture2D image = new Texture2D(2, 2);
        WWW www = new WWW(filePath);
        yield return www;
        www.LoadImageIntoTexture(image);
        yield return null;
    }
}

