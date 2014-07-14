using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

/*
 * Author : Thomas Rouvinez
 * Description : class to handle the endgame menu.
 */
public class EndGameMenu : MonoBehaviour {

	// ------------------------------------------------------------------
	// Variables.
	// ------------------------------------------------------------------

	public GUISkin skinBackground;
	public GUISkin skinbutton;
	public Camera hudCamera;
	public AudioSource engine;
	public AudioSource endGameAmbiant;
	
	private int width = Screen.width;
	private int height = Screen.height;
	private string playerName = "Your Name";
	private bool sent = false;
	private bool received = false;
	private bool invoked = false;
	private bool guiEnabled = false;

	// ------------------------------------------------------------------
	// Game loop.
	// ------------------------------------------------------------------

	void Update (){
		if(GameConfiguration.Instance.ended && invoked == false){
			Invoke ("EnableGUI", 2.5f);
			invoked = true;
		}
	}

	void EnableGUI(){
		guiEnabled = true;
		engine.audio.Stop();
		endGameAmbiant.audio.Play();
	}

	// ------------------------------------------------------------------
	// GUI.
	// ------------------------------------------------------------------

	void OnGUI(){
		if(guiEnabled){
			hudCamera.enabled = false;

			// Put background image and title.
			GUI.skin = skinBackground;
			GUI.Box(new Rect(0, 0, width, height), "");
			GUI.Label(new Rect((width * 0.1f),(height * 0.05f),(width * 0.8f),(height * 0.25f)), "<size=" + (width * 0.07f) + ">GAME OVER</size>");

			// Score display.
			GUI.Label(new Rect((width * 0.2f),(height * 0.35f),(width * 0.6f),(height * 0.15f)), "<size=" + (width * 0.03f) + ">SCORED : " + GameConfiguration.Instance.score.ToString() + "</size>");

			// Highscore publish button.
			GUI.skin = skinbutton;

			if(sent == false){
				// Submit button.
				if(GUI.Button(new Rect((width * 0.65f),(height * 0.45f),(width * 0.15f),(height * 0.15f)), "<size=" + (width * 0.02f) + ">SEND</size>")){
					// Send highscore.
					HighscoreSaver.postScore(playerName.ToUpper(), GameConfiguration.Instance.score.ToString(), this);
					sent = true;

					// Write best score internally.
					if((long) (PlayerPrefs.GetFloat("highscore", 0f)) < GameConfiguration.Instance.score){
						PlayerPrefs.SetFloat("highscore", (float) GameConfiguration.Instance.score);
					}
				}

				// Name textfield.
				playerName = GUI.TextField(new Rect((width * 0.25f),(height * 0.45f),(width * 0.4f),(height * 0.15f)), playerName, 20);
			}
			else{
				GUI.Label(new Rect((width * 0.25f),(height * 0.45f),(width * 0.5f),(height * 0.15f)), "<size=" + (width * 0.02f) + ">HIGHSCORE SUBMITTED</size>");
			}

			// Control buttons.
			if(GUI.Button(new Rect ((width * 0.65f), (height * 0.8f),(width * 0.3f),(height * 0.1f)), "<size=" + (width * 0.02f) + ">RESTART ></size>")){
				Application.LoadLevel(1);
			}

			if(GUI.Button(new Rect ((width * 0.05f), (height * 0.8f),(width * 0.3f),(height * 0.1f)), "<size=" + (width * 0.02f) + ">< MAIN MENU</size>")){
				Application.LoadLevel(0);
			}

			GUI.skin = null;
		}
	}
}