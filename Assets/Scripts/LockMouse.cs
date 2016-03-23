using UnityEngine;
using System.Collections;

public static class LockMouse {

	public static void Lock(){
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	public static void Unlock(){
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
}