using UnityEngine;
using System.Collections;

/*
 * Author : Thomas Rouvinez
 * Description : class to handle the in-game pause menu.
 * 
 */
public class PauseMenu : MonoBehaviour {
	
	// -------------------------------------------------------------------------------------
	// Variables.
	// -------------------------------------------------------------------------------------

	public GUISkin pauseBackground;
	public GUISkin pauseResume;
	public GUISkin pauseMainMenu;

	public AudioSource engine;
	public AudioSource levelMusic;

	private int width;
	private int height;
	
	// -------------------------------------------------------------------------------------
	// Game loop.
	// -------------------------------------------------------------------------------------

	void Start(){
		width = (Screen.width / 6) * 4;
		height = (Screen.height / 2);
		GameConfiguration.Instance.paused = false;
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape) & GameConfiguration.Instance.paused == false & GameConfiguration.Instance.ended == false){
			GameConfiguration.Instance.paused = true;
			engine.Pause();
			levelMusic.Pause();
			Time.timeScale = 0;
		}
		
		else if(Input.GetKeyDown(KeyCode.Escape) & GameConfiguration.Instance.paused == true){
			GameConfiguration.Instance.paused = false;
			engine.Play();
			levelMusic.Play();
			Time.timeScale = 1;
		}
	}

	// -------------------------------------------------------------------------------------
	// GUI.
	// -------------------------------------------------------------------------------------

	void OnGUI () {
		if(GameConfiguration.Instance.paused){
			// Put background image.
			GUI.skin = pauseBackground;
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
			
			// Resume button.
			GUI.skin = pauseResume;
			if(GUI.Button(new Rect (width,height,(Screen.width * 0.3f),(Screen.height * 0.1f)), "<size=" + (width * 0.04f) + ">RESUME</size>"))
			{
				Time.timeScale = 1;
				GameConfiguration.Instance.paused = false;
			}
			
			// Main Menu button.
			GUI.skin = pauseMainMenu;
			if(GUI.Button(new Rect (width,height + (Screen.height * 0.1f),(Screen.width * 0.3f),(Screen.height * 0.1f)), "<size=" + (width * 0.04f) + ">MAIN MENU</size>"))
			{
				Time.timeScale = 1;
				Application.LoadLevel(0);
			}
		}
	}
}