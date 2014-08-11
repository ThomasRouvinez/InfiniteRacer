using UnityEngine;
using System.Collections;

/*
 * Author : Arnaud Durand
 * Description : class to handle the ship's movement.
 * 
 */	
public class PlayerBehaviour : MonoBehaviour {

	// ----------------------------------------------------------------------------
	// Variables.
	// ----------------------------------------------------------------------------

	public float radius = 20f;
	public float depth = 12;
	public float cameraRadius = 6f;
	public float speed = 0f;
	public float maxSpeed;
	public float acceleration;
	public float deceleration;

	public float motion;
	public bool onCollision = false;
	public Transform rotationAxis;

	private bool ft = false;	// FAFUK with variable's name ???
	private float positionOnOrbit = 0f;
	private float shiftAmount = 0f;
	private bool inputEnabled = false;

	// Scripts references.
	public GameObject navigation;
	public GameObject camera;

	private HUD hudScript;
	private NavigationController navigationScript;
	private float relativeVelocity;

	// ----------------------------------------------------------------------------
	// Setter.
	// ----------------------------------------------------------------------------

	public void Shift(float shiftAmount){
		this.shiftAmount = shiftAmount;
	}

	public void InputEnabled(bool enabled){
		this.inputEnabled = enabled;
	}

	public bool InputEnabled(){
		return inputEnabled;
	}

	// ----------------------------------------------------------------------------
	// Start && Late Update.
	// ----------------------------------------------------------------------------

	//public static float horizontalSpeed=200f;
	void Start () {
		maxSpeed = 0.9f;
	}

	void Update() {
		// Reactivity adaptation.
		relativeVelocity = ((GameConfiguration.Instance.speed - GameConfiguration.Instance.startSpeed) / 150);
		
		acceleration = 1.5f + relativeVelocity;	// 1.9f
		deceleration = 2.0f + relativeVelocity;	// 0.9f
	}

	// Rotate arround.
	private Vector3 RotateAroundPoint(Vector3 point, Vector3 pivot, Quaternion angle){
		return angle * ( point - pivot) + pivot;
	}

	// User input management.
	void LateUpdate () {
		if ((inputEnabled == true && (Input.GetKey ("left")||(Input.GetMouseButton(0)&&Input.mousePosition.x<Screen.width/2))&&(speed > -maxSpeed)))
       		speed = speed - acceleration * Time.deltaTime;

		else if( (inputEnabled == true && (Input.GetKey ("right")||(Input.GetMouseButton(0)&&Input.mousePosition.x>Screen.width/2))&&(speed < maxSpeed)))
       		speed = speed + acceleration * Time.deltaTime;

     	else {
       		if(speed > deceleration * Time.deltaTime)
         		speed = speed - deceleration * Time.deltaTime;
       		else if(speed < -deceleration * Time.deltaTime)
         		speed = speed + deceleration * Time.deltaTime;
       		else
        		speed = 0;
		}
		
		motion = speed * Time.deltaTime;
		motion += shiftAmount;

		shiftAmount = 0f;

		positionOnOrbit += motion;		
		positionOnOrbit = (positionOnOrbit +1) %1;

		// If no collision allow the user to move in the tubes.
		if (!onCollision){
			transform.position=RotateAroundPoint(new Vector3(0f, -radius, 0f), rotationAxis.transform.position, rotationAxis.transform.rotation*Quaternion.Euler(0f,0f,positionOnOrbit*360));
			transform.rotation=rotationAxis.transform.rotation*Quaternion.Euler(0f,0f,positionOnOrbit*360);
		}		
	}
}