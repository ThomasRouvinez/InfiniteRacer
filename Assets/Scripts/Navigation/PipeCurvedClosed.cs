﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	private int[] indexes;
	
	// -------------------------------------------------------------------------------------
	// Upon creation, initialize the obstacles.
	// -------------------------------------------------------------------------------------
	
	void Awake(){
		curved = true;
		
		obstacles = new GameObject[2];
		densities = new int[2];
		sizes = new float[2];
		indexes = new int[2];
		
		obstacles[0] = obsPC2;		densities[0] = 20;		sizes[0] = 0.1f;	indexes[0] = 1;
		obstacles[1] = obsPC5;		densities[1] = 90;		sizes[1] = 0.3f;	indexes[1] = 4;
		
		StartCoroutine(spawn(this.getObstaclesPool));
	}
	
	// -------------------------------------------------------------------------------------
	// Obstacle strategy.
	// -------------------------------------------------------------------------------------
	
	private IEnumerator spawn(ObstaclesPooling obstaclesPool){
		
		// Compute maximum density allowed based on the current speed (speed up => less obstacles).
		densityMax = getDensity();
		
		// Determine which obstacles are unlocked (based on distance travelled, progressively introduces obstacles).
		unlocks = GameConfiguration.Instance.thresholdIndex > 3 ? 3 : GameConfiguration.Instance.thresholdIndex;
		
		// Spawn strategy.
		while(density < densityMax && minPosition < .85f){	// Synchronize progressive introduction of obstacles.

			// Select obstacle and position.
			random = (int) (Random.Range(0f, Mathf.Clamp(unlocks, 0, 2)));

			switch (random){
			case 0:
				tempRotation = Random.Range(0, 360);
				minPosition += 0.1f;
				break;
				
			case 1:
				minPosition += .05f;
				break;
				
			default:
				minPosition += .1f;
				break;
			}
			
			// Spawn obstacle.
			StartCoroutine(spawnObstacle(obstaclesPool, this, indexes[random], minPosition, new Vector3(0f, 0f, tempRotation)));
			
			// Update strategy factors.
			density += densities[random];
			minPosition += sizes[random];
		}
		
		yield return null;
	}
}