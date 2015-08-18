using UnityEngine;
using System.Collections;

public class PipeBehaviour : NavigationBehaviour { 

	// -------------------------------------------------------------------------------------
	// Functions for all the children to be able to spawn obstacles.
	// -------------------------------------------------------------------------------------

	// Returns a value between 280 and 100 units of available density for obstacles in a pipe.
	public int getDensity(){
		return (int) (GameConfiguration.Instance.maxSpeed - (GameConfiguration.Instance.speed * 0.5f));
	}

	// Generic function to spawn an obstacle (a spawnable).
	public IEnumerator spawnObstacle(Transform obstacle, Transform parent, float position, Vector3 rotation){
		/*Transform spawnable = Instantiate(obstacle, spline.GetPositionOnSpline(position), spline.GetOrientationOnSpline(position)) as Transform;
		spawnable.transform.parent = parent;
		yield return new WaitForSeconds(.1f);
		
		spawnable.transform.Rotate(rotation,Space.Self);*/
		yield return null;
	}
}