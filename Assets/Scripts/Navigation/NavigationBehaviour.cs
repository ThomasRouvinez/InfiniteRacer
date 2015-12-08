using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Author: Arnaud Durand
 * Do NOT modify this script without author acknowledgement
 */
[RequireComponent(typeof (Spline))]
public class NavigationBehaviour : MonoBehaviour {
	
	// --------------------------------------------------------------------------------------
	// Variables.
	// --------------------------------------------------------------------------------------

	public float torque = 0f;
	public bool curved;
	public bool opened;
	public int length;
	
	private List<PooledObstacle> obstaclesList;

	private GameObject configuration;
	private ObstaclesPooling obstaclesPool;

	// --------------------------------------------------------------------------------------
	// Initialization.
	// --------------------------------------------------------------------------------------

	void Start(){
		obstaclesList = new List<PooledObstacle>();

		configuration = GameObject.Find("Configuration");
		obstaclesPool = configuration.GetComponent<ObstaclesPooling>();
	}

	// --------------------------------------------------------------------------------------
	// Getter/Setter.
	// --------------------------------------------------------------------------------------

	public Spline spline{
		get {return GetComponent<Spline>();}
	}

	public ObstaclesPooling getObstaclesPool{
		get {return obstaclesPool;}
	}

	public List<PooledObstacle> getObstaclesList{
		get {return obstaclesList;}
	}
}