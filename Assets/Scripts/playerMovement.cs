using UnityEngine;
using System.Collections;

public class playerMovement : MonoBehaviour {

	private float runSpeed;
	private float walkSpeed;
	private float jumpForce;

	private bool canRecieveInput;
	private bool canJump;

	private bool isMoving;

	void Start () {
		canRecieveInput = true;
		runSpeed = 600;
		walkSpeed = 3;
		jumpForce = 10;	
	}
	
	void FixedUpdate () {



		if (canRecieveInput) {
			float mHorizontal = Input.GetAxis("Horizontal");
			float mVertical = Input.GetAxis("Vertical");

			if (!Input.GetButton("Crouch")) {
				GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * mVertical * runSpeed * Time.deltaTime);
			}
			else {
				GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * mHorizontal * walkSpeed * Time.deltaTime);
			}



		}
	} // end FixedUpdate
	
}
