using UnityEngine;
using System.Collections;

public class USELESS : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<ParticleEmitter> ().localVelocity = new Vector3 (0,200,0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
