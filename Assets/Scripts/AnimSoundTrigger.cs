using UnityEngine;
using System.Collections;

/*
 * Author : Thomas Rouvinez
 * Description : class to handle main menu's animation
 * 				sounds for the ships' flyby.
 * 
 */
public class AnimSoundTrigger : MonoBehaviour {

	public AudioSource flyby1;
	public AudioSource flyby2;

	void triggerFlyBy1()
	{
		flyby1.audio.Play();
	}

	void triggerFlyBy2()
	{
		flyby2.audio.Play();
	}
}