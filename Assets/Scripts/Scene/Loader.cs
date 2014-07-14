using UnityEngine;
using System.Collections;

/*
 * Author : Thomas Rouvinez
 * Description : Class to handle the loading screen to launch
 * 				a new game from the main menu.
 * 
 */
public class Loader : MonoBehaviour {

	// -------------------------------------------------------------------------------------
	// Variables.
	// -------------------------------------------------------------------------------------

	public Texture loadingTexture;
	public GUISkin skin;
	public GameObject navigation;
	public AudioSource music;
	public AudioSource engine;
	public GameManager gameManager;

	// -------------------------------------------------------------------------------------
	// Stop the time during loading.
	// -------------------------------------------------------------------------------------

	void Awake(){
		Time.timeScale = 0;
	}

	// -------------------------------------------------------------------------------------
	// GUI.
	// -------------------------------------------------------------------------------------

	void OnGUI () {
		// Background image.
		GUI.DrawTexture (new Rect(0,0,Screen.width,Screen.height), loadingTexture, ScaleMode.StretchToFill);
		
		// Create the "START" button when loading is complete.
		GUI.skin = skin;

		if(GUI.Button (new Rect (0, 0,Screen.width, Screen.height), "") || Input.GetKeyDown(KeyCode.Return)){
			// Reset values of the game.
			gameManager.ResetConfiguration();

			// Start the music and engine sound.
			if(GameConfiguration.Instance.gameMusicOn == true){
				music.audio.Play();
			}

			engine.audio.Play();

			// Start game.
			Time.timeScale = 1;
			Destroy (this);
		}
	}
}