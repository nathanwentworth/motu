using UnityEngine;
using System.Collections;
using System;

public class scavItems : IComparable<scavItems> {

	public string nameItem;
	public string nameReadable;

	public scavItems(string newNameItem, string newNameReadable) {
		nameItem = newNameItem;
		nameReadable = newNameReadable;
	}

	public int CompareTo(scavItems other) {
		if (other == null) {
			return 1;
		}
		return 0;
	}
}
