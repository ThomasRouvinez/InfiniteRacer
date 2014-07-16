using UnityEngine;
using System.Collections;

public class ShipLoader : MonoBehaviour {

	public GameObject[] ships;
	public GameObject player;

	private Transform ship;

	// Spawn the good prefab for the ship.
	void Start () {
		switch (PlayerPrefs.GetInt("ship", 0)){
		case 0:
			ship = Instantiate(ships[0], new Vector3(4.6f, 30f, 15.7f), ships[0].transform.rotation) as Transform;
			GameObject.Find("UAF886(Clone)").transform.parent = player.transform;
			break;
			
		case 1:
			ship = Instantiate(ships[1], new Vector3(0f, -5.3f, -0.3f), ships[1].transform.rotation) as Transform;
			GameObject.Find("H-92(Clone)").transform.parent = player.transform;
			break;
		}
	}
}
