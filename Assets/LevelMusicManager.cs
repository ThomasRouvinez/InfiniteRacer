using UnityEngine;
using System.Collections;

public class LevelMusicManager : MonoBehaviour {
	
	public AudioClip loopB;
	public AudioSource musicSource;
	private bool changed = false;

	// Update is called once per frame.
	void changeMusic(){
		musicSource.Stop();
		musicSource.clip = loopB;
		musicSource.loop = true;
		musicSource.Play();
		changed = true;
	}

	void Update(){
		if(musicSource.time > 121f){
			changeMusic();
			Debug.Log("MUSic CHANGED");
		}
	}
}