using UnityEngine;
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
    private int photoNumber;

    void Start()
    {
        photoNumber = System.IO.Directory.GetFiles(string.Format("{0}/Resources/", Application.dataPath)).Length;
        print(photoNumber); 
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
            string filename = ScreenShotName(currentGameMode, photoNumber);
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
            photoNumber = System.IO.Directory.GetFiles(string.Format("{0}/Resources/", Application.dataPath)).Length;
            print(photoNumber);
        }
    }

        public static string ScreenShotName(int currentGameMode, int photoNumber){
            return string.Format("{0}/Resources/MOTUv2_{1}_{2}.png",
                             Application.dataPath,
                             currentGameMode, photoNumber);
    }
}

