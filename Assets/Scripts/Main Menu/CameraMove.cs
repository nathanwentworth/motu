using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

	//Private Variables

	// void Awake () {  }
	// void Start () {  }
	void Update () {
        transform.Rotate(new Vector3(0, Time.deltaTime * 2, 0));
    }
	// void FixedUpdate () {  }

	// void OnTriggerEnter(Collider other) {  }
	// void OnTriggerExit(Collider other) {  }
	// void OnTriggerStay(Collider other) {  }

	// void OnCollisionEnter(Collision collision) {  }
	// void OnCollisionExit(Collision collision) {  }

}
