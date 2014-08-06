using UnityEngine;
using System.Collections;

public class PipeCurvedClosed : PipeBehaviour {
	
	// -------------------------------------------------------------------------------------
	// Variables.
	// -------------------------------------------------------------------------------------
	
	private float minPosition = 0.1f;
	private int density = 0;					// Current density level.
	private int densityMax = 0;					// Maximum density of obstacles allowed.
	private int unlocks = 0;					// Determines which obstacles are unlocked.
	private int random;
	private float tempPosition = 0f;
	private float tempRotation = 0f;
	private int lastObstacle;
	private float lastRotation;
	
	public GameObject obsPC2;
	public GameObject obsPC5;
	
	private GameObject[] obstacles;
	private int[] densities;
	private float[] sizes;
	
	// -------------------------------------------------------------------------------------
	// Upon creation, initialize the obstacles.
	// -------------------------------------------------------------------------------------
	
	void Awake(){
		curved = true;
		
		obstacles = new GameObject[2];
		densities = new int[2];
		sizes = new float[2];
		
		obstacles[0] = obsPC2;		densities[0] = 20;		sizes[0] = 0.1f;
		obstacles[1] = obsPC5;		densities[1] = 90;		sizes[1] = 0.3f;
		
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
		
		// Spawn strategy.
		while(density < densityMax && minPosition < .85f){	// Synchronize progressive introduction of obstacles.

			// Select obstacle and position.
			random = (int) (Random.Range(0f, Mathf.Clamp(unlocks, 0, 2)));

			if(random == 0){
				tempRotation = Random.Range(0, 360);
				minPosition += 0.1f;
			}
			
			// Spawn obstacle.
			StartCoroutine(spawnObstacle(obstacles[random].transform, this.transform, minPosition, new Vector3(0f, 0f, tempRotation)));
			
			// Update strategy factors.
			density += densities[random];
			minPosition += sizes[random];
		}
		
		yield return null;
	}
}