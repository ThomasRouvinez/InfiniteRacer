using UnityEngine;
using System;
using System.Collections;

/*
 * Author: Arnaud Durand
 * Do NOT modify this script without author acknowledgement
 */
public class NavigationController : MonoBehaviour {

	// --------------------------------------------------------------------------------------
	// Variables.
	// --------------------------------------------------------------------------------------

	private float splinePosition = 0f;
	private NavigationBehaviour[] pipes;
	private int pipeIdx = 0;

	public NavigationBehaviour[] pipePrefabs;
	public NavigationBehaviour startPipePrefab;
	public Camera camera;

	public Transform rotationAxis;
	public PlayerBehaviour player;

	float GenerateTorque(){
		return UnityEngine.Random.Range(0,12) * 60f;
	}

	void Start(){
		pipes = new NavigationBehaviour[5];

		Vector3 nextPosition = Vector3.zero;
		Quaternion nextOrientation = Quaternion.identity;
		Spline currentSpline = null;

		NavigationBehaviour pipePrefab = startPipePrefab;
		
		for(int i = 0 ; i < pipes.Length; i++){
			pipes[i] = Instantiate(pipePrefab, nextPosition, nextOrientation) as NavigationBehaviour;

			pipes[i].transform.parent=transform;

			pipes[i].torque = i != 0 ? GenerateTorque(): 0 ;
			pipes[i].transform.Rotate(new Vector3(0,0,pipes[i].torque), Space.Self);

			currentSpline = pipes[i].spline;
			nextPosition = currentSpline.GetPositionOnSpline(1f);
			nextOrientation = currentSpline.GetOrientationOnSpline(1f);

			pipePrefab = pipePrefabs[UnityEngine.Random.Range(0,pipePrefabs.Length)];
		}
	}
	
	void Update (){
		Spline spline = pipes[pipeIdx].spline;
		splinePosition += (GameConfiguration.Instance.speed * Time.deltaTime) / spline.Length;

		if(splinePosition > 1f)/*Change current tube*/{
			GameConfiguration.Instance.distance += pipes[pipeIdx].length;	// For rewards.
			float exceedingDistance = (splinePosition % 1) * spline.Length;
			Vector3 sOffset = -spline.GetPositionOnSpline(1f);
			
			foreach (NavigationBehaviour tube in pipes){
				tube.transform.position += sOffset;
			}
			
			RespawnBlocks(); //Warning: change tubeIdx
			spline = pipes[pipeIdx].spline;
			splinePosition = exceedingDistance/spline.Length;
			player.Shift(-pipes[pipeIdx].torque/360);
		}
		
		Vector3 offset = -spline.GetPositionOnSpline(splinePosition);
		foreach (NavigationBehaviour tube in pipes){
            tube.transform.position += offset;
        }

		rotationAxis.rotation=spline.GetOrientationOnSpline(splinePosition);

		// FOV modification depending on the type of tubes.
		if(pipes[pipeIdx].curved == true){
			camera.fieldOfView = Mathf.Clamp((camera.fieldOfView + Time.deltaTime * 4f), 80f, 90f);
		}
		else{
			camera.fieldOfView = Mathf.Clamp((camera.fieldOfView - Time.deltaTime * 4f), 80f, 90f);
		}
	}

	// --------------------------------------------------------------------------------------
	// Functions.
	// --------------------------------------------------------------------------------------

	void RespawnBlocks(){
		Destroy(pipes[pipeIdx].gameObject);

		int prvIdx = (pipeIdx + pipes.Length-1) % pipes.Length;
		Spline previousSpline = pipes[prvIdx].GetComponent<Spline>();

		pipes[pipeIdx] = Instantiate(pipePrefabs[UnityEngine.Random.Range(0,pipePrefabs.Length)], previousSpline.GetPositionOnSpline(1f), previousSpline.GetOrientationOnSpline(1f)) as NavigationBehaviour;
		pipes[pipeIdx].transform.parent = transform;

		// Avoid pipes from going opposite directions.
		pipes[pipeIdx].torque = Mathf.Clamp(GenerateTorque(), previousSpline.GetOrientationOnSpline(1f).z -90, previousSpline.GetOrientationOnSpline(1f).z +90);
		
		pipes[pipeIdx].transform.Rotate(new Vector3(0,0,pipes[pipeIdx].torque), Space.Self);
		pipeIdx = (pipeIdx +1) % pipes.Length;
	}
}