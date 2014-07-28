using UnityEngine;
using System.Collections;

/*
 * Authors : Tomas Rouvinez, Didier Aeberhard
 * Creation date : 16.10.2013
 * Last Modified : 17.10.2013
 *
 * Description : class to handle all animations of the ship.
 */
public class ShipAnimator : MonoBehaviour {
	
	// --------------------------------------------------------------
	// Variables.
	// --------------------------------------------------------------
	
	// Game Objects.
	public GameObject propellerLeft;
	public GameObject propellerRight;
	public GameObject wingLeft;
	public GameObject wingRight;
	public AudioSource engineSource;

	private Quaternion rotLeft0;
	private Quaternion rotRight0;
	private Vector3 posLeft0;
	private Vector3 posRight0;
	private Vector3 rotWingLeft;
	private Vector3 rotWingRight;
	private float engineRate = 3.25f;

	// Scripts.
	private PlayerBehaviour playerScript;
	
	// --------------------------------------------------------------
	// Update.
	// --------------------------------------------------------------
	
	// Use this for initialization
	void Start ()
	{		
		rotLeft0 = wingLeft.transform.localRotation;
		posLeft0 = wingLeft.transform.localPosition;
		rotRight0 = wingRight.transform.localRotation;
		posRight0 = wingRight.transform.localPosition;

		// Scripts.
		playerScript = GetComponent<PlayerBehaviour> ();
	}
	
	// Update is called once per frame.
	void Update ()
	{
		rotatePropellers(); 	// Rotate the propellers.
		moveWings();			// Move the wings
	}
	
	// --------------------------------------------------------------
	// Ship animations.
	// --------------------------------------------------------------
	
	// Rotate the propellers.
	void rotatePropellers()
	{
		propellerLeft.transform.Rotate(Vector3.up, Time.deltaTime * 100000);
		propellerRight.transform.Rotate(Vector3.up, Time.deltaTime * 100000);
	}
	
	//Moves the wings when right/left keys are pressed.
	void moveWings()
	{
		//Max and min angle between rotation of the wings during lateral movements
		float wingDeltaAngle = 20.0f;
		//Speed rotation of the wings during lateral movements.
		float speedDeltaAngle = 2.5f;
		
		//Setting x and y Euler angle during lateral movements.
		rotWingLeft.x = 0f;
		rotWingLeft.y = 0f;	
		rotWingRight.x = 0f;
		rotWingRight.y = 0f;
		
		//If statement of the lateral movement.
		if (playerScript.InputEnabled() == true && (Input.GetKey("left") || (Input.GetMouseButton(0) && Input.mousePosition.x < Screen.width/2)))
		{
			//Rotate wings for left lateral movement.
			rotWingLeft.z +=  speedDeltaAngle;
			rotWingRight.z +=  speedDeltaAngle;
			rotWingLeft.z = Mathf.Clamp(rotWingLeft.z, -wingDeltaAngle, wingDeltaAngle);
			rotWingRight.z = Mathf.Clamp(rotWingRight.z, 270f - wingDeltaAngle, 270f + wingDeltaAngle);
			wingLeft.transform.localEulerAngles = rotWingLeft;
			wingRight.transform.localEulerAngles = rotWingRight;

			// Adapt engine pitch.
			engineSource.pitch = Mathf.Lerp(engineSource.pitch, 1.2f, engineRate * Time.deltaTime);
		}
		else if (playerScript.InputEnabled() == true && (Input.GetKey("right") || (Input.GetMouseButton(0) && Input.mousePosition.x > Screen.width/2)))
		{
			//Rotate wings for right lateral movement.
			rotWingLeft.z -= speedDeltaAngle;
			rotWingRight.z -= speedDeltaAngle;
			rotWingLeft.z = Mathf.Clamp(rotWingLeft.z, -wingDeltaAngle, wingDeltaAngle);
			rotWingRight.z = Mathf.Clamp(rotWingRight.z, 270f - wingDeltaAngle, 270f + wingDeltaAngle);
			wingLeft.transform.localEulerAngles = rotWingLeft;
			wingRight.transform.localEulerAngles = rotWingRight;

			// Adapt engine pitch.
			engineSource.pitch = Mathf.Lerp(engineSource.pitch, 1.2f, engineRate * Time.deltaTime);
		}
		else
		{
			//Rotate back to initial rotation state (also for position).
			wingRight.transform.localRotation = Quaternion.Slerp(wingRight.transform.localRotation, rotRight0, Time.deltaTime * 4);
			wingRight.transform.localPosition = Vector3.Lerp(wingRight.transform.localPosition, posRight0, Time.deltaTime * 4);
			
			wingLeft.transform.localRotation = Quaternion.Slerp(wingLeft.transform.localRotation, rotLeft0, Time.deltaTime * 4);
			wingLeft.transform.localPosition = Vector3.Lerp(wingLeft.transform.localPosition, posLeft0, Time.deltaTime * 4);
			
			rotWingLeft.z =  0f;
			rotWingRight.z = 270f;

			// Revert engine audio to normal pitch.
			engineSource.pitch = Mathf.Lerp(engineSource.pitch, 1f, engineRate * Time.deltaTime);
		}
	}
}