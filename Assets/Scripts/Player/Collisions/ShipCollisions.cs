using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
/*
 * Author : Thomas Rouvinez
 * Description : class to handle the collisions on the ship.
 */

public class ShipCollisions : MonoBehaviour {

	// ------------------------------------------------------------------------
	// Variables.
	// ------------------------------------------------------------------------

	public BoxCollider colliderLeft;
	public BoxCollider colliderRight;
	public GameObject sparksLeft;
	public GameObject sparksRight;
	public AudioSource coinNoise;
	public AudioSource music;
	public AudioSource explosion;
	public AudioSource energy;
	public AudioSource slowDown;
	
	public GameObject camera;
	public GameObject ship;

	public Detonator smokePrefab;
	public GameManager gameManager;

	private PlayerBehaviour player;
	private float delta = 0f;
	private float delta2 = 0f;

	// ------------------------------------------------------------------------
	// Start.
	// ------------------------------------------------------------------------

	void Start(){
		player = ship.GetComponent<PlayerBehaviour>();
	}

	void Update(){
		energy.volume = Mathf.Clamp(energy.volume - Time.deltaTime, 0f, 1f);
		slowDown.volume = Mathf.Clamp(energy.volume - Time.deltaTime, 0f, 1f);
	}

	// ------------------------------------------------------------------------
	// Collisions.
	// ------------------------------------------------------------------------

	// Ship collisions detection.
	void OnTriggerEnter(Collider collision){
		// Coins detection.
		if(collision.gameObject.tag == "Recharge"){
			GameConfiguration.Instance.energy = Mathf.Clamp((GameConfiguration.Instance.energy + (Time.deltaTime * (GameConfiguration.Instance.speed / 10))), 0f, 100f);
			GameConfiguration.Instance.score += Time.deltaTime * 20f;

			energy.volume = 0.5f;

			if(! energy.isPlaying || (Time.time - delta > 1.5f)){
				energy.Play();
				delta = Time.time;
			}
		}
		
		else if(collision.gameObject.tag == "SlowDown"){
			GameConfiguration.Instance.speed = Mathf.Clamp((GameConfiguration.Instance.speed - (Time.deltaTime * 15f)), 120f, 300f);

			slowDown.volume = 1f;
			
			if(! slowDown.isPlaying || (Time.time - delta2 > 1.5f)){
				slowDown.Play();
				delta2 = Time.time;
			}
		}

		// Falling from half pipes detection.
		else if(collision.gameObject.name == "ColliderHalfPipe"){
			collision.enabled = false;
			player.enabled=false;

			GameConfiguration.Instance.speed = 0;
			GameConfiguration.Instance.causeOfDeath = 1;
			Rigidbody rigidBody = GetComponent<Rigidbody>();

			rigidbody.isKinematic=false;
			rigidbody.AddForce(transform.localPosition*50000f+transform.forward*20000f*GameConfiguration.Instance.speed);

			StartCoroutine(WaitAndFall(0.3f));
		}

		/*else if(collision.gameObject.tag == "Powerup"){
			collision.gameObject.transform.parent = gameObject.transform;
			collision.gameObject.renderer.enabled = false;
			collision.enabled=false;
			gameManager.addPowerup((Powerup) collision.GetComponent(typeof(Powerup)));
		} */ 

		// Lost the game.
		else{
			foreach(Collider collider in GetComponents<Collider>())
				collider.enabled = false;

			GameConfiguration.Instance.speed = 0f;
			GameConfiguration.Instance.causeOfDeath = 2;

			// Destroy the ship.
			player.onCollision = true;
			player.motion = 0f;

			explosion.audio.Play();
			StartCoroutine(WaitAndExplode(0f));

			GameConfiguration.Instance.ended = true;
			music.audio.Stop();
		}
	}

	IEnumerator WaitAndFall(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		
		Destroy(rigidbody);
		Destroy(GetComponent<ShipAnimator>());
		
		yield return new WaitForSeconds(1f);
		
		// Get ended game screen.
		GameConfiguration.Instance.ended = true;
	}

	/*
	 * Author : Arnaud Durand
	 */
	IEnumerator WaitAndExplode(float waitTime) {
		yield return new WaitForSeconds(waitTime);

		Destroy(rigidbody);
		Destroy(GetComponent<ShipAnimator>());

		Detonator[] parts=GetComponentsInChildren<Detonator>();

		foreach (Detonator part in parts){
			if (part.gameObject == gameObject)
				continue;

			part.gameObject.AddComponent<Rigidbody>();
			part.transform.parent=null;
			part.rigidbody.AddExplosionForce(200f,transform.position-transform.forward*10,20f);
		}

		GetComponent<Detonator>().Explode();

		yield return new WaitForSeconds(0.25f);
		foreach (Detonator part in parts){
			part.Explode();
		}

		// Instantiate a detonator game object where the bomb is.
		//Instantiate (smokePrefab, transform.position-transform.forward*5, Quaternion.identity);  

		yield return new WaitForSeconds(3f);

		Destroy(gameObject);
	}
}