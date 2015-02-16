using UnityEngine;
using System.Collections;

public class LevelMusic : MonoBehaviour {

	public AudioClip loopA;
	public AudioClip loopB;

	public AudioSource musicSource;

	// Use this for initialization
	void Start () {
		musicSource.Play();
	}
	
	// Update is called once per frame
	void FixedUpdate(){
		if(musicSource.time == 20.0){
			musicSource.clip = loopB;
			musicSource.Play();
		}
	}
}