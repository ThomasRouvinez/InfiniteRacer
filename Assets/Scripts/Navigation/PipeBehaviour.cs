using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PipeBehaviour : NavigationBehaviour { 

	// -------------------------------------------------------------------------------------
	// Functions for all the children to be able to spawn obstacles.
	// -------------------------------------------------------------------------------------

	// Returns a value between 280 and 100 units of available density for obstacles in a pipe.
	public int getDensity(){
		return (int) (GameConfiguration.Instance.maxSpeed - (GameConfiguration.Instance.speed * 0.5f));
	}

	// Generic function to spawn an obstacle (a spawnable).
	public IEnumerator spawnObstacle(ObstaclesPooling obstaclesPool, NavigationBehaviour pipe, int obstacleIndex, float position, Vector3 rotation){

		if(obstaclesPool == null){
			Debug.Log("FUCK YOU !");
		}

		// Take the correct obstacle from the pool.
		GameObject obstacle = obstaclesPool.getOrCreate(obstacleIndex);
		obstacle.transform.position = spline.GetPositionOnSpline(position);
		obstacle.transform.rotation = spline.GetOrientationOnSpline(position);
		obstacle.transform.Rotate(rotation,Space.Self);

		// Write its reference in the pipe's list of obstacles.
		pipe.getObstaclesList.Add(new PooledObstacle(obstacleIndex, obstacle));

		// Set the correct parent.
		obstacle.transform.parent = pipe.transform;

		yield return null;
	}
}