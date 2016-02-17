// by @torahhorse

using UnityEngine;
using System.Collections;

public class LockMouse : MonoBehaviour
{	
	void Start()
	{
  // 	Cursor.visible = false;
		// Cursor.lockState = CursorLockMode.Locked;
	}

    void Update()
    {
    	// lock when mouse is clicked
    	// if( Input.GetMouseButtonDown(0) && Time.timeScale > 0.0f ) {
	    // 	Cursor.visible = false;
	 			// Cursor.lockState = CursorLockMode.Locked;
    	// }
    
    	// unlock when escape is hit
        if  (Input.GetKeyDown(KeyCode.Escape) )
        {
		    	Cursor.visible = true;
	    		Cursor.lockState = CursorLockMode.None;
        }
    }
}