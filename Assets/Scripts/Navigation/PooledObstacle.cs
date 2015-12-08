using UnityEngine;
using System.Collections;

public class PooledObstacle : MonoBehaviour {

	// -------------------------------------------------------------------------------------
	// Variables.
	// -------------------------------------------------------------------------------------

	private int index;
	private GameObject obstacle;
	
	// -------------------------------------------------------------------------------------
	// Constructor.
	// -------------------------------------------------------------------------------------

	public PooledObstacle(int index, GameObject obstacle){
		this.index = index;
		this.obstacle = obstacle;
	}

	// -------------------------------------------------------------------------------------
	// Getter - setters.
	// -------------------------------------------------------------------------------------

	public int Index { get; set ; }
	public GameObject Obstacle { get; set ; }
}
