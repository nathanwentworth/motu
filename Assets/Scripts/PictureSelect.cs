using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PictureSelect : MonoBehaviour {

    public int photoNumber = -1;
    private GalleryManager gallery;
    
    void Start()
    {
        gallery = GameObject.Find("Main Camera").GetComponent<GalleryManager>();
    }

	public void Highlight()
    {
        Debug.Log("Selected picture " + photoNumber.ToString()+".");
        gallery.currentPhotoHighlighted = photoNumber;
    }
}
