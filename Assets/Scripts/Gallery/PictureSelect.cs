using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PictureSelect : MonoBehaviour {

    public int photoNumber = 0;
    private GalleryManager gallery;
    
    void Start()
    {
        gallery = GameObject.Find("Main Camera").GetComponent<GalleryManager>();
    }

    public void OnMouseEnter()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OnMouseExit()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

	public void OnClick()
    {
        gallery.ViewPhoto(photoNumber);
    }
}
