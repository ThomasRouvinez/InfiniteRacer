using UnityEngine;
using System.Collections;

public class PipeCurved : PipeBehaviour {
	
	// -------------------------------------------------------------------------------------
	// Variables.
	// -------------------------------------------------------------------------------------
	
	private float minPosition = 0.2f;
	private float maxPosition = 0.5f;
	private float currentPosition;
	private int density = 0;					// Current density level.
	private int densityMax = 0;					// Maximum density of obstacles allowed.
	private int unlocks = 0;					// Determines which obstacles are unlocked.
	private int random;
	private float tempPosition = 0f;
	private float tempRotation = 0f;

	private GameObject[] obstacles;
	private int[] densities;

	// -------------------------------------------------------------------------------------
	// Upon creation, initialize the obstacles.
	// -------------------------------------------------------------------------------------

	public void Awake(){
		curved = true;
		currentPosition = minPosition;

		StartCoroutine(spawn());
	}

	// -------------------------------------------------------------------------------------
	// Obstacle strategy.
	// -------------------------------------------------------------------------------------

	private IEnumerator spawn(){
		// Compute maximum density allowed based on the current speed (speed up => less obstacles).
		densityMax = getDensity();

		// Determine which obstacles are unlocked (based on distance travelled, progressively introduces obstacles).
		unlocks = GameConfiguration.Instance.thresholdIndex > 2 ? 3 : GameConfiguration.Instance.thresholdIndex;

		yield return null;
	}
}
