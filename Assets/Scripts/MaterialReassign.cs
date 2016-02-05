using UnityEngine;
using System.Collections;
using UnityEditor;

public class MaterialReassign : MonoBehaviour {

	void Update(){
		AssetDatabase.Refresh ();
		Texture newpic = Resources.Load("Screenshot", typeof(Texture)) as Texture; 
		gameObject.GetComponent<Renderer> ().material.SetTexture ("_MainText", newpic); 
	}
}
