using UnityEngine;
using System.Collections;

public class rotationCheck : MonoBehaviour {

  public Transform other;
  void Update() {
    if (other) {
      Vector3 forward = transform.TransformDirection(Vector3.forward);
      Vector3 toOther = other.position - transform.position;
      Debug.Log(Vector3.Dot(forward, toOther));
      // if (Vector3.Dot(forward, toOther) < 0) {
      //   print("The other transform is behind me!");      	
      // }
      // else if (Vector3.Dot(forward, toOther) >= 0) {
      // 	print("less than 1, greater than 0");
      // }
      // else if (Vector3.Dot(forward, toOther) >= 1) {
      // 	print("greater than or equal to 1");
      // }
    }
  }
}
