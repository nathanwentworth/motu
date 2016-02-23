using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{

    [Header("Enable Movement")]
    public bool enableMovement = true;

    [Header("GameObject References")]
    public GameObject pauseBG;
    public GameObject mainBeastUI;
    public GameObject ui_CameraUI;
    public GameObject mainCam;
    public GameObject notification_UI;
    public Text notification_TXT;
    public GameObject[] animalPanels;

    [Header("Audio References")]
    public AudioSource MemCard1Audio;
    public AudioSource ForestAmbience1;
    public AudioSource ForestAmbienceLPF;
    public AudioSource ItemPickup;
    public AudioSource[] CameraClicks;
    public AudioMixerSnapshot Default;
    public AudioMixerSnapshot InTent;

    //Private Fields
    LockMouse mouse = new LockMouse();
    private bool startResetCameraUI;
    private bool mainUIOn;
    private float resetCameraUI;
    private bool aimDown = false;
    private bool isAnotherUIActive = false;
    //Tutorial bools
    private bool instructions1 = true;
    private bool instructions3 = false;
    private bool isW = false;
    private bool isA = false;
    private bool isS = false;
    private bool isD = false;
    private bool isJump = false;
    private bool isCrouch = false;
    private bool picOfStretchDog = false;

    //Non-Input but Referenced
    [HideInInspector]
    public string whatAnimal;
    [HideInInspector]
    public bool changeToForestLPF = false;
    [HideInInspector]
    public bool changeToForest = false;
    [HideInInspector]
    public GameObject MemCard1Object;
    [HideInInspector]
    public bool instructions2 = false;
    [HideInInspector]
    public bool nearMemCard1 = false;
    [HideInInspector]
    public bool instructions4 = false;

    void Start()
    {

        //Give Variables Properties
        mainBeastUI.SetActive(false);
        for (int i = 0; i < animalPanels.Length; i++)
        {
            animalPanels[i].SetActive(false);
            if (i == 0)
                animalPanels[i].SetActive(true);
        }
        mouse.Lock();
        AudioListener.pause = false;
        ui_CameraUI.SetActive(false);
        Default.TransitionTo(2f);
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
        //Open Beastiary
        if (Input.GetButtonDown("HUD") && !mainUIOn && !isAnotherUIActive)
        {
            mainBeastUI.SetActive(true);
            mainUIOn = true;
            isAnotherUIActive = true;
            mouse.Unlock();
            enableMovement = false;
        }
        //Close Beastiary
        else if (Input.GetButtonDown("HUD") && mainUIOn)
        {
            isAnotherUIActive = false;
            mainBeastUI.SetActive(false);
            isAnotherUIActive = false;
            mainUIOn = false;
            mouse.Lock();
            enableMovement = true;
        }
        //Pause the game
        if (Input.GetKeyDown(KeyCode.Escape) && !isAnotherUIActive && Time.timeScale == 1.0f)
        {
            isAnotherUIActive = true;
            Time.timeScale = 0.0f;
            mouse.Unlock();
            pauseBG.SetActive(true);
            AudioListener.pause = true;
        }
        //Unpause game
        else if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0.0f)
        {
            isAnotherUIActive = false;
            Time.timeScale = 1.0f;
            mouse.Lock();
            pauseBG.SetActive(false);
            AudioListener.pause = false;
        }
        //Take a picture but only if the camera is aimed down
        if (Input.GetButtonDown("Fire1") && aimDown)
        {
            int i = Random.Range(0, 2);
            CameraClicks[i].Play();
            //Disable Camera UI
            ui_CameraUI.SetActive(false);
            //Send out a ray that returns a string
            Raycasting();

            //Take the screenshot and save it depending on what was hit in the raycast
            if (whatAnimal == "stretchdog")
            {
                if (Application.isEditor)
                {
                    Application.CaptureScreenshot("Assets\\Resources\\StretchDog.png");
                    whatAnimal = "";
                }
                else
                {
                    Application.CaptureScreenshot("Resources\\StretchDog.png");
                    whatAnimal = "";
                }

                startResetCameraUI = true;
            }
            else
            {
                if (Application.isEditor)
                {
                    Application.CaptureScreenshot("Assets\\Resources\\Other.png");
                    whatAnimal = "";
                }
                else
                {
                    Application.CaptureScreenshot("Resources\\Other.png");
                    whatAnimal = "";
                }

                startResetCameraUI = true;
            }
        }

        //OTHER CONDITIONAL STATEMENTS
        //Play MemCard Audio
        if (nearMemCard1 && Input.GetButtonDown("Submit"))
        {
            ItemPickup.Play();
            MemCard1Audio.Play();
            MemCard1Object.SetActive(false);
        }
        //Start the camera reset timer
        if (startResetCameraUI)
        {
            resetCameraUI += Time.deltaTime;
        }
        //Reset the camera
        if (resetCameraUI >= 0.1f)
        {
            if (instructions3)
                notification_UI.SetActive(true);
            ui_CameraUI.SetActive(true);
            startResetCameraUI = false;
            resetCameraUI = 0.0f;
        }
        //Chnage Background Ambience if inside
        if (changeToForestLPF)
        {
            //InTent.TransitionTo(0.1f);
            ForestAmbience1.spatialBlend += Time.deltaTime;
            if (ForestAmbience1.spatialBlend == 1)
            {
                changeToForestLPF = false;
            }
        }
        if (changeToForest)
        {
            //Default.TransitionTo(0.1f);
            ForestAmbience1.spatialBlend -= Time.deltaTime;
            if (ForestAmbience1.spatialBlend == 0)
            {
                changeToForest = false;
            }
        }

        if (notification_UI.activeSelf)
        {
            if (instructions1)
            {
                isAnotherUIActive = true;
                enableMovement = true;
                //Declare Local Variables
                float inputX = Input.GetAxis("Horizontal");
                float inputY = Input.GetAxis("Vertical");
                string strW;
                string strA;
                string strS;
                string strD;

                //Check if keys were pressed
                if (inputX > 0)
                {
                    isD = true;
                }
                else if (inputX < 0)
                {
                    isA = true;
                }

                if (inputY > 0)
                {
                    isW = true;
                }
                else if (inputY < 0)
                {
                    isS = true;
                }

                //Change color of text
                if (isW)
                {
                    strW = "<color=green>W</color> ";
                }
                else
                {
                    strW = "<color=red>W</color> ";
                }
                if (isA)
                {
                    strA = "<color=green>A</color> ";
                }
                else
                {
                    strA = "<color=red>A</color> ";
                }
                if (isS)
                {
                    strS = "<color=green>S</color> ";
                }
                else
                {
                    strS = "<color=red>S</color> ";
                }
                if (isD)
                {
                    strD = "<color=green>D</color> ";
                }
                else
                {
                    strD = "<color=red>D</color> ";
                }

                //Update Text
                notification_TXT.text = "Press " + strW + strA + strS + strD + "to move.\n Use the <color=red>Mouse</color> to look around.";

                //Check if all keys have been pressed
                if (isW && isA && isS && isD)
                {
                    notification_TXT.text = "Press <color=red>Space</color> to jump.";

                    if (Input.GetButtonDown("Jump"))
                        isJump = true;

                    if (isJump)
                    {
                        notification_TXT.text = "Press <color=red>Left Shift</color> to crouch.";

                        if (Input.GetButtonDown("Crouch"))
                            isCrouch = true;

                        if (isCrouch)
                        {
                            isAnotherUIActive = false;
                            instructions1 = false;
                            notification_UI.SetActive(false);
                        }
                    }
                }
            }
            if (!instructions1 && instructions2)
            {
                notification_TXT.text = "Press <color=red>Right Click</color> or <color=red>Alt</color> to look into your camera's viewfinder.";

                if (aimDown)
                {
                    notification_TXT.text = "Press <color=red>Left Click</color> when looking into your camera's viewfinder to take a picture. Try taking a picture of that creature over there.";

                    if (picOfStretchDog)
                    {
                        instructions2 = false;
                        instructions3 = true;
                        notification_UI.SetActive(false);
                    }
                }
            }
            if (!instructions2 && instructions3)
            {
                notification_TXT.text = "Press <color=red>Tab</color> to take out your tablet and view the picture you just took.";
                if (mainUIOn)
                {
                    notification_TXT.text = "Good shot! Whenever you take a new picture the old one will be overwritten. Press <color=red>Tab</color> again to close your tablet.";
                    instructions4 = true;
                }
                if (!mainUIOn && instructions4)
                {
                    instructions3 = false;
                    notification_UI.SetActive(false);
                }
            }
            if (!instructions3 && instructions4)
            {
                notification_TXT.text = "<color=red>Memory Cards</color> are items that contain useful data such as audiologs and creature information.";
                if (nearMemCard1)
                {
                    notification_TXT.text = "Press <color=red>E</color> to pickup and use items.";
                    if (Input.GetButton("Submit"))
                    {
                        instructions4 = false;
                        nearMemCard1 = false;
                        notification_UI.SetActive(false);
                    }
                }
            }
        }
    }

    //Switch Beastiary Menu on Button Press
    public void menuSwitch(int menuPos)
    {
        animalPanels[menuPos].SetActive(true);
        for (int i = 0; i < animalPanels.Length; i++)
        {
            if (i != menuPos)
                animalPanels[i].SetActive(false);
        }
    }

    //Send out a ray to see what animal were taking a pic of
    void Raycasting()
    {
        RaycastHit hit;
        Ray camRay = new Ray(mainCam.transform.position, mainCam.transform.forward);
        if (Physics.Raycast(camRay, out hit, 500))
        {
            if (hit.collider.tag == "StretchDog")
            {
                whatAnimal = "stretchdog";
                picOfStretchDog = true;
                Debug.Log(hit.distance);
            }
            else
            {
                whatAnimal = "other";
            }
        }
    }
}
