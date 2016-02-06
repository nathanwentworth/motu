using UnityEngine;
using System.Collections;

public class crouchCam : MonoBehaviour {

	private Vector3 topPos;
	private Vector3 botPos;

  public float speed;

  void Start() {
  	topPos = new Vector3(transform.localPosition.x, 0.5f, transform.localPosition.z);
  	botPos = new Vector3(transform.localPosition.x, -0.5f, transform.localPosition.z);
  }
	

	void Update () {
		if (Input.GetButton("Crouch")) {
			transform.localPosition = Vector3.Lerp(transform.localPosition, botPos, Time.deltaTime * speed);
		}
		else {
			transform.localPosition = Vector3.Lerp(transform.localPosition, topPos, Time.deltaTime * speed);
		}
	}
}
