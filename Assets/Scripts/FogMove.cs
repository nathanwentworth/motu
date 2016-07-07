using UnityEngine;
using System.Collections;

public class FogMove : MonoBehaviour {

    public float multiplyer;
	
	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(gameObject.GetComponent<Renderer>().material.mainTextureOffset.x + (Time.deltaTime * multiplyer), 0);
	}
}
