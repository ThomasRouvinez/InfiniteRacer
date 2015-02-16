using UnityEngine;
using System.Collections;

public class LevelMusicManager : MonoBehaviour {
	
	public AudioClip loopB;
	public AudioSource musicSource;

	// Update is called once per frame
	void Update () {
		if(GameConfiguration.Instance.started == true & !musicSource.isPlaying){
			if(GameConfiguration.Instance.ended != true && GameConfiguration.Instance.paused != true){
				musicSource.Stop();
				musicSource.clip = loopB;
				musicSource.loop = true;
				musicSource.Play();
			}
		}
	}
}