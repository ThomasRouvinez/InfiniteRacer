using UnityEngine;
using System.Collections;

/*
 * Author : Thomas Rouvinez
 */

public class VFXPC5 : MonoBehaviour {
	
	void Update () {
		transform.Rotate(Vector3.forward, Time.deltaTime * -25);
	}
}