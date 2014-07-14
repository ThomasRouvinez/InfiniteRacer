using UnityEngine;
using System.Collections;

/*
 * Author : Thomas Rouvinez
 * Description : class to adapt the speed of the starfield to the ship's speed.
 * 
 */
public class ParticleSpeedController : MonoBehaviour {

	private ParticleEmitter starfield;

	// Use this for initialization
	void Start () {
		starfield = GetComponent<ParticleEmitter>();
	}
	
	// Update is called once per frame
	void Update () {
		starfield.particleEmitter.localVelocity = new Vector3(0,GameConfiguration.Instance.speed,0);
	}
}