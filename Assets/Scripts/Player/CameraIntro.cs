using UnityEngine;
using System.Collections;

/*
 * Author : Thomas Rouvinez
 * Description : class to handle the management of the introduction
 * 				animation and magages the controls.
 * 
 */
public class CameraIntro : MonoBehaviour {

	// -------------------------------------------------------------
	// Variables.
	// -------------------------------------------------------------

	public Camera camera;
	public GameObject ship;

	private Animator animation;
	private PlayerBehaviour playerScript;

	// -------------------------------------------------------------
	// Start.
	// -------------------------------------------------------------

	void Start(){
		animation = camera.GetComponent<Animator> ();
		playerScript = ship.GetComponent<PlayerBehaviour> ();
	}

	// -------------------------------------------------------------
	// Disable intro cutscene.
	// -------------------------------------------------------------

	void disableAnimation(){
		// Game is starting.
		animation.enabled = false;
		playerScript.InputEnabled(true);
		GameConfiguration.Instance.energy = 100f;
	}
}