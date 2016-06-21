using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour {

    public MovementObjective objective1;

	void Start () {
        LockMouse.Lock();
        Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
        if (objective1 == null)
        {
            return;
        }
        else {
            if (objective1.MovementObjectiveTriggered)
            {
                Debug.Log("Objective 1 Triggered");
                objective1.MovementObjectiveTriggered = false;
            }
        }
	}
}
