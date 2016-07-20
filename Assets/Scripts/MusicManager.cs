using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour {

	[Header("Music")]
	public AudioSource Hymn;
	public AudioSource Rain;
	public AudioSource Emerge;
    public AudioSource Cpio;
    public AudioSource Confirm;
    public AudioSource Deny;
	private int PlaylistNumber;
	[Header("CameraClicks")]
	private int WhichClick;
	public AudioSource CameraClick1;
	public AudioSource CameraClick2;
	public AudioSource CameraClick3;
    public AudioSource[] Footsteps;
    public AudioSource Landing;

    private List<AudioSource> AllMusic = new List<AudioSource>();

    public static MusicManager Instance { get; private set; }
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }
        else {

            // Here we save our singleton instance
            Instance = this;

            // Furthermore we make sure that we don't destroy between scenes (this is optional)
            DontDestroyOnLoad(gameObject);

            AllMusic.Add(Hymn);
            AllMusic.Add(Rain);
            AllMusic.Add(Emerge);
            AllMusic.Add(Cpio);
        }

    }
    // Use this for initialization

    public IEnumerator Playlist(){
		PlaylistNumber = Mathf.RoundToInt(Random.Range (1, 4));
		print (PlaylistNumber);
		switch (PlaylistNumber) {
		case 1:
                while (true)
                {
                    Hymn.Play();
                    yield return new WaitForSeconds(Hymn.clip.length);
                    Rain.Play();
                    yield return new WaitForSeconds(Rain.clip.length);
                    Emerge.Play();
                    yield return new WaitForSeconds(Emerge.clip.length);
                }
		case 2:
                while (true)
                {
                    Rain.Play();
                    yield return new WaitForSeconds(Rain.clip.length);
                    Emerge.Play();
                    yield return new WaitForSeconds(Emerge.clip.length);
                    Hymn.Play();
                    yield return new WaitForSeconds(Hymn.clip.length);
                }
		case 3:
                while (true)
                {
                    Emerge.Play();
                    yield return new WaitForSeconds(Emerge.clip.length);
                    Hymn.Play();
                    yield return new WaitForSeconds(Hymn.clip.length);
                    Rain.Play();
                    yield return new WaitForSeconds(Rain.clip.length);
                }
		}
	}

    public void MainMenuMusic()
    {
        Cpio.Play();
        Cpio.loop = true;
    }

    public void StopAllMusic()
    {
        Cpio.Stop();
        Rain.Stop();
        Emerge.Stop();
        Hymn.Stop();
    }

    public void StopPlayList()
    {
        StopCoroutine(Playlist());
    }

    public void PlayConfirm()
    {
        Confirm.Play();
    }

    public void PlayDeny()
    {
        Deny.Play();
    }

	public void PlayCameraClick () {
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

    public void PlayFootstep(int number)
    {
        Footsteps[number].Play();
    }

    public void PlayLanding()
    {
        Landing.Play();
    }

    public void SetMusicVolume(float volume)
    {
        for(int i = 0; i < AllMusic.Capacity; i++)
        {
            AllMusic[i].volume = volume;
        }
    }
}
