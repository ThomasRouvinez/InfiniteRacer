using UnityEngine;
using System.Collections;

/*
 * Author : Thomas Rouvinez
 */

public class VFXFan : MonoBehaviour {
	
	void Update () {
		transform.Rotate(Vector3.up, Time.deltaTime * -150);
	}
}