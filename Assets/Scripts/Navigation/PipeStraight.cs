using UnityEngine;
using System.Collections;

public class PipeStraight : PipeBehaviour {
	
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

	public GameObject obsPC1;
	public GameObject obsPC2;
	public GameObject obsPC3;
	public GameObject obsPC4;

	private GameObject[] obstacles;
	private int[] densities;

	// -------------------------------------------------------------------------------------
	// Upon creation, initialize the obstacles.
	// -------------------------------------------------------------------------------------

	public void Awake(){
		curved = false;
		currentPosition = minPosition;

		obstacles = new GameObject[4];
		densities = new int[4];

		obstacles[0] = obsPC1;	densities[0] = 30;
		obstacles[1] = obsPC2;	densities[1] = 75;
		obstacles[2] = obsPC3;	densities[2] = 100;
		obstacles[3] = obsPC4;	densities[3] = 100;

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

		// Randomly select the main obstacle type.
		random = (int) (Random.Range(0f, (float) unlocks +1));
		yield return new WaitForSeconds(.2f);

		// Spawn the main obstacle with prefered position.
		tempPosition = Random.Range(minPosition, minPosition + 0.2f);
		tempRotation = Random.Range(0, 12) * 30;
		
		StartCoroutine(spawnObstacle(obstacles[random].transform, this.transform, tempPosition, new Vector3(0f, 0f, tempRotation)));
		density += densities[random];

		yield return new WaitForSeconds(.2f);

		// Fill randomly the available density left.

		yield return null;
	}
}
