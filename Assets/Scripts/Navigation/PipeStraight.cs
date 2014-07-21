using UnityEngine;
using System.Collections;

public class PipeStraight : PipeBehaviour {
	
	// -------------------------------------------------------------------------------------
	// Variables.
	// -------------------------------------------------------------------------------------
	
	private float minPosition = 0.1f;
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
	private float[] sizes;

	// -------------------------------------------------------------------------------------
	// Upon creation, initialize the obstacles.
	// -------------------------------------------------------------------------------------

	public void Awake(){
		curved = false;
		currentPosition = minPosition;

		obstacles = new GameObject[4];
		densities = new int[4];
		sizes = new float[4];

		obstacles[0] = obsPC1;	densities[0] = 50;		sizes[0] = 0.2f;
		obstacles[1] = obsPC2;	densities[1] = 80;		sizes[1] = 0.3f;
		obstacles[2] = obsPC3;	densities[2] = 100;		sizes[2] = 0.4f;
		obstacles[3] = obsPC4;	densities[3] = 100;		sizes[3] = 0.6f;

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

		// Spawn the obstacles.
		while(density < densityMax && minPosition < 1f){
			yield return new WaitForSeconds(.2f);
			random = (int) (Random.Range(0f, (float) unlocks +1));
			tempPosition = Random.Range(minPosition, minPosition + 0.2f);
			tempRotation = (Random.Range(0, 12) * 30) + 15;
			
			StartCoroutine(spawnObstacle(obstacles[random].transform, this.transform, tempPosition, new Vector3(0f, 0f, tempRotation)));
			density += densities[random];
			minPosition += sizes[random];
		}

		yield return new WaitForSeconds(.2f);

		// Fill randomly the available density left.

		yield return null;
	}
}
