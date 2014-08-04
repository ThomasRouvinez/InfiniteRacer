using UnityEngine;
using System.Collections;

/*
 * Author : Thomas Rouvinez
 * Description : class to handle the loading screen itself.
 * 
 */
public class LoadingScreen : MonoBehaviour {
	
	// Variables.
	//private int loading = 1;
	public Texture loadingTexture;
	public float loadingState;
	
	void Start() {
        Application.LoadLevel(2);
    }
	
	void OnGUI () {
		// Put background image.
		GUI.DrawTexture (new Rect(0,0,Screen.width,Screen.height), loadingTexture, ScaleMode.StretchToFill);
	}
}