using UnityEngine;
using System.Collections;

public class CannonController : MonoBehaviour {

	public ParticleEmitter muzzleBig;
	public ParticleEmitter muzzleLong;
	private AudioSource fireShot;

	// Use this for initialization
	void Start () {
		fireShot = GetComponent<AudioSource>();
	}

	// Sound effects.
	public void fire()
	{
		fireShot.audio.Play ();
		//muzzleBig.Emit ();
		//muzzleLong.Emit ();
	}
}