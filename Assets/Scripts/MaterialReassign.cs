using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class MaterialReassign : MonoBehaviour
{

    private string SDPath;
    private string other;
    public Manager gameManager;

    void Start()
    {
        SDPath = "file://" + Application.dataPath + "/Resources/StretchDog.png";
        other = "file://" + Application.dataPath + "/Resources/Other.png";
        StartCoroutine("LoadImageStart");
    }

    void Update()
    {

    }

    IEnumerator LoadImageStretchDog()
    {
        Texture2D image = new Texture2D(2, 2);
        WWW www = new WWW(SDPath);
        yield return www;
        www.LoadImageIntoTexture(image);
        gameObject.GetComponent<RawImage>().texture = image;
        yield return null;
    }

    IEnumerator LoadImageOther()
    {
        Texture2D image = new Texture2D(2, 2);
        WWW www = new WWW(other);
        yield return www;
        www.LoadImageIntoTexture(image);
        gameObject.GetComponent<Renderer>().material.mainTexture = image;
        yield return null;
    }
}
