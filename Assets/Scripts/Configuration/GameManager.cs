using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/*
 * Authors: Arnaud Durand, Thomas Rouvinez
 * Description: handle speed, score and rewards.
 */
public class GameManager : MonoBehaviour {
	public GUISkin powerupSkins;
	public float startTimer;
	public float timer;
	
	private float speedCheck;

	private int width;
	private int height;
	private bool rewardOn = false;

	private LTRect reward;
	private LTRect rewardLogo;
	public GUISkin skinReward;
	
	public AudioSource sourceReward;
	public AudioClip[] audioRewards;
		
	// ----------------------------------------------------------------------
	// Update score.
	// ----------------------------------------------------------------------

	// Use this for initialization
	void Awake () {
		ResetConfiguration();
	}

	void Start() {
		startTimer = timer = Time.time;
		width = Screen.width;
		height = Screen.height;

		reward = new LTRect((width * 0.1f), (height * 0.07f), (width * 0.8f), (height * 0.3f));
		rewardLogo = new LTRect((width * 0.3f), 0, (width * 0.4f), (height * 0.2f));
		reward.color.a = 0f;

		// Reload the settings.
		GameConfiguration.Instance.gameMusicOn = PlayerPrefs.GetInt("gameMusicOn", 1) == 1 ? true : false;
		GameConfiguration.Instance.menuMusicOn = PlayerPrefs.GetInt("menuMusicOn", 1) == 1 ? true : false;
		GameConfiguration.Instance.hardcoreMode = PlayerPrefs.GetInt("gameMode", 1) == 1 ? true : false;
	}

	void Update () {
		// Update and detect each second.
		if(Time.time - timer >= 1f && GameConfiguration.Instance.ended == false){
			timer = Time.time;
			GameConfiguration.Instance.score += 1 + (GameConfiguration.Instance.hardcoreMode == true ? 1:0);
			speedCheck = GameConfiguration.Instance.speed + Mathf.Sqrt(Time.deltaTime) * 10;
			GameConfiguration.Instance.speed = Mathf.Clamp(speedCheck, 120, 250);
		}

		StartCoroutine(checkDistance());

		if(GameConfiguration.Instance.paused == false){
			GameConfiguration.Instance.energy -= Time.deltaTime * 1.2f;
		}

		if(GameConfiguration.Instance.energy < 0f){
			GameConfiguration.Instance.ended = true;
			GameConfiguration.Instance.causeOfDeath = 0;
		}
	}
	
	void OnGUI(){
		// To display the Rewards if needed.
		if(rewardOn == true){
			GUI.skin = skinReward;
			GUI.Box(rewardLogo.rect, "");
			GUI.Label(reward.rect, "<size=" + (reward.rect.width * 0.08f) + ">" + GameConfiguration.Instance.thresholdValues[GameConfiguration.Instance.thresholdIndex -1] + "</size>");
		}
	}

	public void ResetConfiguration () {
		GameConfiguration.Instance.energy = 100f;
		GameConfiguration.Instance.speed = 120f;
		GameConfiguration.Instance.score = 0;
		GameConfiguration.Instance.distance = 0;
		GameConfiguration.Instance.thresholdIndex = 0;
		GameConfiguration.Instance.paused = false;
		GameConfiguration.Instance.ended = false;
		GameConfiguration.Instance.causeOfDeath = 0;
		GameConfiguration.Instance.musicChanged = false;
		GameConfiguration.Instance.started = false;
	}

	public void Destroy(){
		// Save settings at the end before exiting the application.
		PlayerPrefs.SetInt("gameMusicOn", GameConfiguration.Instance.gameMusicOn == true ? 1:0);
		PlayerPrefs.SetInt("menuMusicOn", GameConfiguration.Instance.gameMusicOn == true ? 1:0);
		PlayerPrefs.SetInt("gameMode", GameConfiguration.Instance.hardcoreMode == true ? 1:0);
	}

	// -------------------------------------------------------------------------------------
	// Check if the reward tag should be displayed or not.
	// -------------------------------------------------------------------------------------

	IEnumerator checkDistance(){
		if(GameConfiguration.Instance.thresholdIndex < 4 && GameConfiguration.Instance.distance > GameConfiguration.Instance.thresholdValues[GameConfiguration.Instance.thresholdIndex]){
			// Give score bonus (distance milestone / 100).
			GameConfiguration.Instance.score += (GameConfiguration.Instance.thresholdValues[GameConfiguration.Instance.thresholdIndex]) / 100;

			// Draw the reward tag for 2 seconds.
			rewardOn = true;
			reward.color.a = 1f;
			++GameConfiguration.Instance.thresholdIndex;

			// Play specific reward sound.
			if(GameConfiguration.Instance.thresholdIndex < audioRewards.Length){
				sourceReward.clip = audioRewards[GameConfiguration.Instance.thresholdIndex];
				sourceReward.Play();
			}
			else{
				sourceReward.clip = audioRewards[audioRewards.Length-1];
				sourceReward.Play();
			}

			StartCoroutine(fadeOut(1.5f, reward));
			
		}

		yield return new WaitForSeconds(1f);
	}

	// Reward disappearence function.
	IEnumerator fadeOut(float waitTime, LTRect reward){
		yield return new WaitForSeconds(waitTime);

		while(reward.color.a > 0){ 
			reward.color.a -= Time.deltaTime;
			yield return null;
		}

		rewardOn = false;
	}
}