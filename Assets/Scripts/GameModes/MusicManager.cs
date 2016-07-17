using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	[Header("Music")]
	public AudioSource Hymn;
	public AudioSource Rain;
	public AudioSource Emerge;
	private int PlaylistNumber;
	[Header("CameraClicks")]
	private int WhichClick;
	public AudioSource CameraClick1;
	public AudioSource CameraClick2;
	public AudioSource CameraClick3;

	// Use this for initialization
	void Start () {
		StartCoroutine ("Playlist");
	}
	
	IEnumerator Playlist(){
		PlaylistNumber = Mathf.RoundToInt(Random.Range (1, 4));
		print (PlaylistNumber);
		switch (PlaylistNumber) {
		case 1:
				Hymn.Play ();
				yield return new WaitForSeconds (Hymn.clip.length);
				Rain.Play ();
				yield return new WaitForSeconds (Rain.clip.length);
				Emerge.Play ();
				break;
		case 2:
				Rain.Play ();
				yield return new WaitForSeconds (Rain.clip.length);
				Emerge.Play ();
				yield return new WaitForSeconds (Emerge.clip.length);
				Hymn.Play ();
				yield return new WaitForSeconds (Hymn.clip.length);
				break;
		case 3:
				Emerge.Play ();
				yield return new WaitForSeconds (Emerge.clip.length);
				Hymn.Play ();
				yield return new WaitForSeconds (Hymn.clip.length);
				Rain.Play ();
				yield return new WaitForSeconds (Rain.clip.length);
				break;
		}
	}

	public void CameraClick () {
		WhichClick = Mathf.RoundToInt(Random.Range (1, 4));
		print (WhichClick);
		switch (WhichClick) {
		case 1:
			CameraClick1.Play ();
			break;
		case 2:
			CameraClick2.Play ();
			break;
		case 3:
			CameraClick3.Play ();
			break;
		}
	}
}
