using UnityEngine;
using System.Collections;

/*
 * Author : Thomas Rouvinez
 * Description : class to send a message to the ship collisions
 * 				in case the left wing is hit.
 */
public class RightWingCollisions : MonoBehaviour {

	public GameObject ship;
	private ShipCollisions collisionScript;

	void Start() 
	{
		collisionScript = ship.GetComponent<ShipCollisions>();
	}
	
	void OnTriggerEnter(Collider collider)
	{
		collisionScript.OnHitRight ();
	}
}