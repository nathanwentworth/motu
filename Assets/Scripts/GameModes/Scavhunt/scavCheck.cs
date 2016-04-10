using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class scavCheck : MonoBehaviour {

	private static System.Random rng = new System.Random();  
	List<scavItems> items = new List<scavItems>();

	public Text instructions;
	public Text timerText;
	public GameObject instructionsPanel;
	public AnimationCurve fade;

	private float menuAnimTimer = 0;
	private float timer = 600;
	private int timerInt;
	private int huntedCount;
	private float min;
	private float sec;
	private bool fadePanel;

	void Start () {
        LockMouse.Unlock();
		Time.timeScale = 0;
		huntedCount = 0;

		items.Add( new scavItems("Cube_1", "First Cube"));
		items.Add( new scavItems("Cube_2", "Second Cube"));
		items.Add( new scavItems("Cube_3", "Third Cube"));
		items.Add( new scavItems("Cube_4", "Fourth Cube"));
		items.Add( new scavItems("Cube_5", "Fifth Cube"));
		items.Add( new scavItems("Cube_6", "Sixth Cube"));

    int n = items.Count;
    while (n > 1) {  
      n--;  
      int k = rng.Next(n + 1);  
      scavItems value = items[k];  
      items[k] = items[n];  
      items[n] = value;  
    }

    for (int i = 0; i < 3; i++) {
    	print (items[i].nameReadable);
    }
    for (int i = items.Count - 1; i >= 3; i--) {
    	items.RemoveAt(i);
    }

    instructions.text = "FIND THESE THINGS: \n" + items[0].nameReadable.ToUpper() + ",\n" + items[1].nameReadable.ToUpper() + ",\n" + items[2].nameReadable.ToUpper();
	}

	void Update() {

		if (Time.timeScale == 1) {
			timer -= Time.unscaledDeltaTime;
			timer = Mathf.Clamp(timer, 0, 600);
			timerInt = (int)timer;
			sec = timerInt % 60;
			min = timerInt / 60;
			if (sec > 9) {
				timerText.text = min + ":" + sec;
			} else {
				timerText.text = min + ":0" + sec;				
			}
		}

		if (timer <= 0 || huntedCount == 3) {
			print ("game over!");
			print (huntedCount);
		}

		if (Input.GetButtonDown("Fire1")) {
			RaycastHit hit;
			Ray hitRay = new Ray(transform.position, transform.forward);
			if (Physics.Raycast(hitRay, out hit)) {
		    for (int i = items.Count - 1; i >= 0; i--) {
    			if (hit.transform.gameObject.name == items[i].nameItem) {
    				print ("cool dude, you hit " + items[i].nameReadable);
    				items.RemoveAt(i);
    				huntedCount++;
					}
    		}
			}		
		}

		if (fadePanel) {
            LockMouse.Lock();
			Time.timeScale = 1;
			menuAnimTimer += Time.unscaledDeltaTime;
			instructionsPanel.GetComponent<CanvasGroup>().alpha = fade.Evaluate(menuAnimTimer);
			if (menuAnimTimer >= 1) 
				fadePanel = false;
		}

	}


	public void FadePanel() {
		fadePanel = true;
	}

}
