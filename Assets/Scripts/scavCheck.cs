using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scavCheck : MonoBehaviour {

	private static System.Random rng = new System.Random();  
	List<scavItems> items = new List<scavItems>();


	void Start () {
		

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
	}
}
