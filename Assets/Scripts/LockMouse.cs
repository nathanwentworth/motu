using UnityEngine;
using System.Collections;

public class LockMouse {

	public void Lock(){
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void Unlock(){
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
}