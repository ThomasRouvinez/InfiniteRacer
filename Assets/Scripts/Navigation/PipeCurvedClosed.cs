using UnityEngine;
using System.Collections;

public class PipeCurvedClosed : PipeBehaviour {
	
	// -------------------------------------------------------------------------------------
	// Variables.
	// -------------------------------------------------------------------------------------
	
	private float minPosition = 0.2f;
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
		
		obstacles[0] = obsPC2;		densities[0] = 40;		sizes[0] = 0.2f;
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
		while(density < densityMax && minPosition < .8f){	// Synchronize progressive introduction of obstacles.
			yield return new WaitForSeconds(.2f);

			// Select obstacle and position.
			random = (int) (Random.Range(0f, Mathf.Clamp(unlocks, 0, 2)));
			tempPosition = Random.Range(minPosition, minPosition + .2f);

			if(random == 0){
				if(random == lastObstacle){
					tempRotation = lastRotation + 45;
					
					lastObstacle = random;
					lastRotation = tempRotation;
				}
				else{
					tempRotation = (Random.Range(0, 12) * 30) + 15;
				}
			}

			if(random == 0){
				minPosition += 0.15f;
			}

			yield return new WaitForSeconds(.2f);
			
			// Spawn obstacle.
			StartCoroutine(spawnObstacle(obstacles[random].transform, this.transform, tempPosition, new Vector3(0f, 0f, tempRotation)));
			
			// Update strategy factors.
			density += densities[random];
			minPosition += sizes[random];
		}
		
		yield return null;
	}
}