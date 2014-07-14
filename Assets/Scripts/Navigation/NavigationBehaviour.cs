using UnityEngine;
using System.Collections;

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
	public bool curved = true;
	public int length;

	// --------------------------------------------------------------------------------------
	// Getter/Setter.
	// --------------------------------------------------------------------------------------

	public Spline spline
	{
		get {return GetComponent<Spline>();}
	}

	public virtual void Awake() {}
}