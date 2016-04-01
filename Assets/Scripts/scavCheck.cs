using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class scavCheck : MonoBehaviour {

	private static System.Random rng = new System.Random();  
	List<scavItems> items = new List<scavItems>();

	public Text instructions;
	public GameObject instructionsPanel;
	public AnimationCurve fade;

	private float timer = 0;
	private bool fadePanel;

	void Start () {

		Time.timeScale = 0;

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

    // foreach (scavItems i in items) {
    //   print (i.nameReadable + " " + i.nameItem);
    // }
    for (int i = 0; i < 3; i++) {
    	print (items[i].nameReadable);
    }

    instructions.text = "FIND THESE THINGS: \n" + items[1].nameReadable.ToUpper() + ",\n" + items[2].nameReadable.ToUpper() + ",\n" + items[3].nameReadable.ToUpper();
	}

	void Update() {
		if (Input.GetButtonDown("Fire1")) {
			RaycastHit hit;
			Ray hitRay = new Ray(transform.position, transform.forward);
			if (Physics.Raycast(hitRay, out hit)) {
		    for (int i = 0; i < 3; i++) {
    			if (hit.transform.gameObject.name == items[i].nameItem) {
    				print ("cool dude, you hit " + items[i].nameReadable);
					} else {
						print ("nah, you hit " + hit.transform.gameObject.name);
					}
    		}
			}		
		}

		if (fadePanel) {
			Time.timeScale = 1;
			timer += Time.unscaledDeltaTime;
			print (timer);
			instructionsPanel.GetComponent<CanvasGroup>().alpha = fade.Evaluate(timer);
			if (timer >= 1) 
				fadePanel = false;
		}

	}


	public void FadePanel() {
		fadePanel = true;
	}

}
