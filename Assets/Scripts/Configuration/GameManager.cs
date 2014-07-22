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
	private PowerupStacker powerups = new PowerupStacker();

	private int width;
	private int height;
	private bool rewardOn = false;

	private LTRect reward;
	private LTRect rewardLogo;
	public GUISkin skinReward;

	/*
	 * Author : Arnaud Durand
	 * Description : handle powerup stacking and triggering.
	 */
	class PowerupStacker {
		public const int size = 3;

		private List<Powerup> items = new List<Powerup>();

		public Texture2D[] icons = new Texture2D[0];

		private void BufferIcons(){
			icons = items.Select(p => p.icon).ToArray();
		}

		public void Push(Powerup item){
			if (items.Count < size){
				Debug.Log(item);
				items.Add(item);
				BufferIcons();
			}
		}

		public void Pop(int itemAtPosition){
			items[itemAtPosition].Trigger();
			items.RemoveAt(itemAtPosition);
			BufferIcons();
		}
	}
		
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

		reward = new LTRect((width * 0.1f), (height * 0.15f), (width * 0.8f), (height * 0.3f));
		rewardLogo = new LTRect((width * 0.3f), (height * 0.08f), (width * 0.4f), (height * 0.2f));
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
			speedCheck = GameConfiguration.Instance.speed + Mathf.Sqrt(Time.deltaTime)*8;
			GameConfiguration.Instance.speed = Mathf.Clamp(speedCheck, 90, 300);
		}

		StartCoroutine(checkDistance());
	}
	
	void OnGUI(){
		if (!GameConfiguration.Instance.ended){
			GUI.skin = powerupSkins;

			for (int i = 0 ; i < powerups.icons.Length; i++){
				if (GUI.Button (new Rect (width - (width / 10) - i * (width / 10), height - (width / 10), (width / 12), (width / 12)), powerups.icons [i])
				    || Input.GetKey (KeyCode.Space) && GameConfiguration.Instance.isOnPowerUp == false)
				{
					powerups.Pop (i);
				}
			}				
		}

		// To display the Rewards if needed.
		if(rewardOn == true){
			GUI.skin = skinReward;
			GUI.Box(rewardLogo.rect, "");
			GUI.Label(reward.rect, "<size=" + (reward.rect.width * 0.08f) + ">" + GameConfiguration.Instance.thresholdValues[GameConfiguration.Instance.thresholdIndex -1] + "</size>");
		}
	}

	public void addPowerup(Powerup powerup){
		powerups.Push(powerup);
	}

	public void ResetConfiguration () {
		GameConfiguration.Instance.speed = 120;
		GameConfiguration.Instance.coins = 0;
		GameConfiguration.Instance.score = 0;
		GameConfiguration.Instance.distance = 0;
		GameConfiguration.Instance.thresholdIndex = 0;
		GameConfiguration.Instance.paused = false;
		GameConfiguration.Instance.ended = false;
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