using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

  public MovementObjective objective1;
  public Text tutText;
  private int tutIndex;
  private bool objTrigger;
  private string[] tutArr = {"Use WASD or the left analog stick to move to the next objective", "Use the mouse or right analog stick to look around", "Use the left mouse button to take photos"};

	void Start () {
    LockMouse.Lock();
    Time.timeScale = 1;
    tutIndex = 0;
    TutorialTextDisp();
	}

  void TutorialTextDisp() {
    print(tutArr[tutIndex]);
    if (tutText == null) {
      return;
    } else {
      tutText.text = tutArr[tutIndex];
    }
    if (tutIndex <= 1 && objTrigger == false) {StartCoroutine(Timer(5));}
  }
	
	void Update () {
    if (objective1 == null) {
      return;
    } else {
      if (objective1.MovementObjectiveTriggered) {
        Debug.Log("Objective 1 Triggered");
        objTrigger = true;
        objective1.MovementObjectiveTriggered = false;
      }
    }
	}

  IEnumerator Timer(float time) {
    yield return new WaitForSeconds(time);
    tutIndex++;
    TutorialTextDisp();
  }
}
