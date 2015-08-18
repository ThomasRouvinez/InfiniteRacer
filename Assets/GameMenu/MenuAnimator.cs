using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

public class MenuAnimator : MonoBehaviour {

	// -------------------------------------------------------------------------------------
	// Variables.
	// -------------------------------------------------------------------------------------

	// Optimizations.
	private float height01, height02, width01, width02, width03; 

	// Layout variables.
	private int width, height, leftMargin, topMargin, spacing, selected;
	private int creditSelected, tempCount, tempRandom, tempRandom,browseShip;

	private bool tmpBool;
	private string hangarSelected;
	public float translationSpeed, scaleSpeed, alphaSpeed, alphaSpeedCredits;

	public AudioSource click, highscoresRefreshButton, backClick;
	public AudioSource menuMusic, spaceAmbiant;
	public Camera camera;
	public GameObject LightBar,DarkBar;
	public GameObject earth;

	public GUISkin skinMenu;
	public GUISkin skinHangarButtons;
	public GUISkin skinBackButton;
	public GUISkin skinHighscoresList;
	public GUISkin skinHighscoresButton;
	public GUISkin skinHighscoresWindow;
	public GUISkin skinRaceScreen;
	public GUISkin skinSettingsScreen;
	public GUISkin skinExitScreen;
	public GUISkin skinCredits;
	public GUISkin hardcore;
	public GUISkin skinHangarSelectedButton;

	public GUISkin[] creditHeads;
	public GUISkin[] shipsScreens;

	private LTRect[] entries;
	private LTRect[] menus;
	private LTRect[] buttons;
	private LTRect backMenu;

	private string[] exitMessages;
	private string[] creditsStrings;
	private string[] creditsAttributionStrings;

	private Vector2 top;
	private Vector2 topSize;
	private Vector2 hidden;

	private List<HighscoreSaver.Highscore> highscores;

	// -------------------------------------------------------------------------------------
	// Game loop.
	// -------------------------------------------------------------------------------------

	void Start(){
		width = Screen.width;
     	height = Screen.height;
		height01 = height * 0.1f;
		height02 = height * 0.2f;
		width01 = width * 0.1f;
		width02 = width * 0.2f;
		width03 = width * 0.3f;

		leftMargin = (int)(width * 0.04f);
		topMargin = (int)(height / 3);
		spacing = (int)((height * 0.1f));
		selected = -1;
		tempCount = -1;
		tmpFontSize = 0;
		browseShip = 0;
		tmpBool = false;
		highscores = null;

		entries = new LTRect[6];
		entries[0] = new LTRect(leftMargin, topMargin, width03,(height * 0.1f));							// Race.
		entries[1] = new LTRect(leftMargin,(topMargin+1*spacing), width03, height01);						// Settings.
		entries[2] = new LTRect(leftMargin,(topMargin+2*spacing), width03, height01);						// Highscores.
		entries[3] = new LTRect(leftMargin,(topMargin+3*spacing), width03, height01);						// Hangar.
		entries[4] = new LTRect(leftMargin,(topMargin+4*spacing), width03, height01);						// Credits.
		entries[5] = new LTRect(leftMargin,(topMargin+5*spacing), width03, height01);						// Exit.

		menus = new LTRect[12];
		menus[0] = new LTRect(width, (height * 0.3f),(width * 0.85f), (height * 0.6f));						// Race container.
		menus[1] = new LTRect(width, (height * 0.3f),(width * 0.85f), (height * 0.6f));						// Exit container.
		menus[2] = new LTRect(width01, height02, (width * 0.9f), (height * 0.8f));							// Credits.
		menus[6] = new LTRect(width, (height * 0.1f), (width * 0.5f), (height * 0.8f));						// Ship's images.
		menus[7] = new LTRect(width01, height, (width * 0.4f), (height * 0.8f));							// Ship's description.
		menus[8] = new LTRect((width * 0.17f), height, (width * 0.62f), (height * 0.7f));					// Highscores container.
		menus[9] = new LTRect(width, (height * 0.3f),(width * 0.85f), (height * 0.6f));						// Settings container.
		menus[10] = new LTRect(width01, (height * 0.3f), (width * 0.9f), (height * 0.7f));					// Credits title + name container.
		menus[11] = new LTRect(width01, (height * 0.4f), (width * 0.9f), (height * 0.6f));					// Credits attributions container.

		buttons = new LTRect[11];
		buttons[0] = new LTRect(-(width * 0.6f), (height * 0.9f),(width * 0.6f),height01);					// Start race button.
		buttons[1] = new LTRect(width, (height * 0.675f),(width * 0.6f),height01);							// Confirm exit button.
		buttons[6] = new LTRect(width + width01, (height * 0.25f), width01, (height * 0.7f));				// Highscore refresh button.
		buttons[7] = new LTRect((width * 0.5f), -(height * 0.15f), (width * 0.5f), height01);				// Ship selection next button.
		buttons[8] = new LTRect((width * 0.5f), height + (height * 0.15f), (width * 0.5f), height01);		// Ship select button.
		buttons[9] = new LTRect(width, height02, (width * 0.42f), height01);								// Normal game mode.
		buttons[10] = new LTRect(width, height02, (width * 0.42f), height01);								// Hardcore game mode.

		backMenu = new LTRect(-width01, height02, width01,(height * 0.8f));
		backMenu.alpha = 0f;

		exitMessages = new string[]{"[Yes] I pretend I have work to do",
									"[Yes] I'm a sissie", 
									"[Yes] This is game is too hard for me",
									"[Yes] I give up easily...",
									"[Yes] I have a weak will..."};
		
		top = new Vector2(0f, 0f);
		topSize = new Vector2((width * 0.5f), height02);
		creditSelected = 2;

		resetBars();
		hangarSelection();

		// Reload the settings.
		GameConfiguration.Instance.gameMusicOn = PlayerPrefs.GetInt("gameMusicOn", 1) == 1 ? true : false;
		GameConfiguration.Instance.menuMusicOn = PlayerPrefs.GetInt("menuMusicOn", 1) == 0 ? false : true;
		GameConfiguration.Instance.shipSelected = PlayerPrefs.GetInt("ship", 0);
	}

	void Update(){
		earth.transform.Rotate(Vector3.up * Time.deltaTime, Space.Self);
		earth.transform.Rotate(Vector3.right * Time.deltaTime, Space.Self);

		StartCoroutine(checkAudio());
	}

	IEnumerator checkAudio(){
		if(GameConfiguration.Instance.menuMusicOn == false && menuMusic.GetComponent<AudioSource>().isPlaying){
			menuMusic.GetComponent<AudioSource>().Stop();
			spaceAmbiant.Play();
		}

		yield return new WaitForSeconds(0.2f);

		if(GameConfiguration.Instance.menuMusicOn == true && !menuMusic.GetComponent<AudioSource>().isPlaying){
			menuMusic.GetComponent<AudioSource>().Play();
			spaceAmbiant.Stop();
		}

		yield return new WaitForSeconds(0.2f);
	}

	// -------------------------------------------------------------------------------------
	// GUI.
	// -------------------------------------------------------------------------------------

	void OnGUI (){

		// ---------------------------------------------------------------------------------
		// Main entries of the menu.
		// ---------------------------------------------------------------------------------

		GUI.skin = skinMenu;

		if(GUI.Button(entries[0].rect, "<size=" + (entries[0].rect.width * 0.148) + ">RACE !</size>")){
			selected = 0; tempCount++;
			select(entries[0]);

			LeanTween.move(menus[0], new Vector2((width * 0.15f), menus[0].rect.y), translationSpeed).setEase(LeanTweenType.easeInOutBack);
			LeanTween.move(buttons[0], new Vector2((width * 0.4f), buttons[0].rect.y), 0.22f);
			LeanTween.move(buttons[9], new Vector2((width * 0.15f), buttons[9].rect.y), 0.22f);
			LeanTween.move(buttons[10], new Vector2((width * 0.58f), buttons[10].rect.y), 0.22f);

		}

		if(GUI.Button(entries[1].rect, "<size=" + (entries[1].rect.width * 0.148) + ">SETTINGS</size>")){
			selected = 1; tempCount++;
			select(entries[1]);

			LeanTween.move(menus[9], new Vector2((width * 0.15f), menus[9].rect.y), translationSpeed).setEase(LeanTweenType.easeInOutBack);
		}

		if(GUI.Button(entries[2].rect, "<size=" + (entries[2].rect.width * 0.148) + ">HIGHSCORES</size>")){
			selected = 2; tempCount++;
			select(entries[2]);

			LoadHighscores();
			LeanTween.move(buttons[6], new Vector2((width * 0.8f), buttons[6].rect.y), translationSpeed * 3).setEase(LeanTweenType.easeInOutBack);
			LeanTween.move(menus[8], new Vector2((width * 0.17f), (height * 0.25f)), translationSpeed);
		}

		if(GUI.Button(entries[3].rect, "<size=" + (entries[3].rect.width * 0.148) + ">HANGAR</size>")){
			selected = 3;
			tempCount++;
			select(entries[3]);

			LeanTween.move(buttons[7], new Vector2(buttons[7].rect.x, 0f), translationSpeed).setEase(LeanTweenType.easeInOutBack);
			LeanTween.move(buttons[8], new Vector2(buttons[8].rect.x, height - height01), translationSpeed).setEase(LeanTweenType.easeInOutBack);
			LeanTween.move(menus[6], new Vector2((width * 0.5f), menus[6].rect.y), translationSpeed * 1.5f);
			LeanTween.move(menus[7], new Vector2(menus[7].rect.x, height02), translationSpeed * 2f);
		}

		if(GUI.Button(entries[4].rect, "<size=" + (entries[4].rect.width * 0.148) + ">CREDITS</size>")){
			selected = 4; tempCount++;
			select(entries[4]);

			LeanTween.move(menus[10], new Vector2(width01, menus[10].rect.y), translationSpeed);
		}

		if(GUI.Button(entries[5].rect, "<size=" + (entries[5].rect.width * 0.15) + ">EXIT</size>")){
			selected = 5; tempCount++; tempRandom = Random.Range(0, exitMessages.Length);
			select(entries[5]);

			LeanTween.move(menus[1], new Vector2((width * 0.15f), menus[1].rect.y), translationSpeed).setEase(LeanTweenType.easeInOutBack);
			LeanTween.move(buttons[1], new Vector2((width * 0.275f), buttons[1].rect.y), translationSpeed).setEase(LeanTweenType.easeInOutBack);
		}

		GUI.skin = skinBackButton;

		if(GUI.Button(backMenu.rect, "")){
			backClick.GetComponent<AudioSource>().Play();
			reset();
		}

		GUI.skin = null;

		// ---------------------------------------------------------------------------------
		// Draw corresponding screens
		// ---------------------------------------------------------------------------------

		switch(selected){
		case 0:
			// Display artwork for game.
			GUI.skin = skinRaceScreen;
			GUI.Box(menus[0].rect, "");	// Race art.
			
			// Start game button.
			GUI.skin = skinHangarButtons;

			if(GUI.Button(buttons[0].rect, "<size=" + (buttons[0].rect.width * 0.04f) + ">[Yes] Start \t\t>>></size>")){
				Application.LoadLevel(1);
			}

			if(GUI.Button(buttons[9].rect, "<size=" + (buttons[0].rect.width * 0.04f) + ">[Normal Mode]</size>")){
				GameConfiguration.Instance.hardcoreMode = false;
				PlayerPrefs.SetInt("gameMode", GameConfiguration.Instance.hardcoreMode == true ? 1:0);
			}

			if(GUI.Button(buttons[10].rect, "<size=" + (buttons[0].rect.width * 0.04f) + ">[Bastard Mode]</size>")){
				GameConfiguration.Instance.hardcoreMode = true;
				PlayerPrefs.SetInt("gameMode", GameConfiguration.Instance.hardcoreMode == true ? 1:0);
			}

			if(PlayerPrefs.GetInt("gameMode",0) == 1){
				GUI.skin = hardcore;
				GUI.Box(new Rect((width * 0.6f), (height * 0.3f), (width * 0.5f), (height * 0.6f)) , "<size=" + ((width * 0.5f) * 0.04f) + ">EXTRA REWARDS</size>");
			}

			GUI.skin = null;
			break;

		case 1:
			// Settings.
			GUI.skin = skinSettingsScreen;
			GUI.Box(menus[9].rect, "");

			tmpFontSize = (int) ((width * 0.5f) * 0.05);

			GUI.Label(new Rect (width02, (height * 0.4f), (width * 0.7f), height01), "<size=" + tmpFontSize + ">" + "ENABLE IN-GAME MUSIC</size>");
			GUI.Label(new Rect (width02, (height * 0.5f), (width * 0.7f), height01), "<size=" + tmpFontSize + ">" + "ENABLE MENU MUSIC</size>");

			          GameConfiguration.Instance.gameMusicOn = GUI.Toggle (new Rect ((width * 0.8f), (height * 0.4f), height01 ,height01), GameConfiguration.Instance.gameMusicOn, "");
			          GameConfiguration.Instance.menuMusicOn = GUI.Toggle (new Rect ((width * 0.8f), (height * 0.5f), height01 ,height01), GameConfiguration.Instance.menuMusicOn, "");

			// Save settings.
			PlayerPrefs.SetInt("gameMusicOn", GameConfiguration.Instance.gameMusicOn == true ? 1:0);
			PlayerPrefs.SetInt("menuMusicOn", GameConfiguration.Instance.menuMusicOn == true ? 1:0);

			GUI.skin = null;
			break;

		case 2:
			// Highscores.
			int padd = (int)(height * 0.24f);
			int listHeight = (int)(height * 0.071f);

			GUI.skin = skinHighscoresWindow;
			GUI.Box(menus[8].rect,"");

			if(this.highscores != null){
				GUI.skin = skinHighscoresList;
				tmpFontSize = (int) ((width * 0.5f) * 0.05);
				int count = 0;
				bool isIn = false;

				foreach (HighscoreSaver.Highscore hs in this.highscores){
					if(count < 10){
						if(long.Parse(hs.score) > (long)PlayerPrefs.GetFloat("highscore", 0f) && count < 9){
							GUI.Box(new Rect((width * 0.18f),padd,(width * 0.05f), listHeight), "<size=" + tmpFontSize + ">" + hs.position + "</size>");
							GUI.Box(new Rect((width * 0.25f),padd,(width * 0.4f), listHeight), "<size=" + tmpFontSize + ">" + hs.name + "</size>");
							GUI.Box(new Rect((width * 0.6f),padd, width02, listHeight), "<size=" + tmpFontSize + ">" + hs.score + "</size>");
							padd += listHeight;
							count ++;
						}
						else{
							if(isIn == false){
								GUI.Box(new Rect((width * 0.18f),padd,(width * 0.05f), listHeight), "<size=" + tmpFontSize + ">>" + ">" + "</size>");
								GUI.Box(new Rect((width * 0.25f),padd,(width * 0.4f), listHeight), "<size=" + tmpFontSize + ">" + "YOUR BEST" + "</size>");
								GUI.Box(new Rect((width * 0.6f),padd, width02, listHeight), "<size=" + tmpFontSize + ">" + (long)PlayerPrefs.GetFloat("highscore", 0f) + "</size>");
								padd += listHeight;
								count ++;
								isIn = true;
							}

							if(count < 10){
								GUI.Box(new Rect((width * 0.18f),padd,(width * 0.05f), listHeight), "<size=" + tmpFontSize + ">" + hs.position + "</size>");
								GUI.Box(new Rect((width * 0.25f),padd,(width * 0.4f), listHeight), "<size=" + tmpFontSize + ">" + hs.name + "</size>");
								GUI.Box(new Rect((width * 0.6f),padd, width02, listHeight), "<size=" + tmpFontSize + ">" + hs.score + "</size>");
								padd += listHeight;
								count ++;
							}
						}
					}
				}

				GUI.skin = null;
			}
			else{
				// If not loaded yet, display a waiting message.
				GUI.skin = skinHighscoresButton;
				GUI.Box(new Rect((width * 0.35f),(height * 0.45f), width03,(height * 0.1f)), "");	// Loading label.
			}

			GUI.skin = skinHighscoresButton;

			if(GUI.Button(buttons[6].rect, "")){	// Refresh button.
				highscoresRefreshButton.GetComponent<AudioSource>().Play();
				LoadHighscores();
			}

			GUI.skin = null;
			break;

		case 3:
			// Hangar.
			GUI.skin = skinHangarButtons;

			// Display selection of ships.
			if(GUI.Button(buttons[7].rect, "<size=" + (buttons[7].rect.width * 0.05f) + ">NEXT</size>")){
				// Select next ship.
				if(browseShip +2 < shipsScreens.Length){
					browseShip += 2;
				}
				else{
					browseShip = 0;
				}

				hangarSelection();
			}

			if(hangarSelected == "SELECTED"){
				GUI.skin = skinHangarSelectedButton;
			}

			if(GUI.Button(buttons[8].rect, "<size=" + (buttons[7].rect.width * 0.05f) + ">" + hangarSelected + "</size>")){
				GameConfiguration.Instance.shipSelected = browseShip / 2;
				PlayerPrefs.SetInt("ship", GameConfiguration.Instance.shipSelected);

				hangarSelection();
			}

			GUI.skin = shipsScreens[browseShip];
			GUI.Box(menus[7].rect, "");	// Ship description.

			GUI.skin = shipsScreens[browseShip+1];
			GUI.Box(menus[6].rect, "");	// Ship Arts.

			GUI.skin = null;
			break;

		case 4:
			// Display the credit pannels.
			GUI.skin = skinCredits;
			tmpFontSize = (int) (menus[2].width * 0.05);

			GUI.Box(menus[2].rect, "");		// Credits AD.

			GUI.skin = null;
			break;

		case 5: 
			// Display exit confirmation menu.
			GUI.skin = skinExitScreen;

			GUI.Box(menus[1].rect, "");	// Confirmation Dialog.

			if(GUI.Button(buttons[1].rect, "<size=" + (buttons[1].rect.width * 0.025f) + ">" + exitMessages[tempRandom] + "</size>")){
				Application.Quit();
			}

			GUI.skin = null;
			break;
		}
	}

	// -------------------------------------------------------------------------------------
	// Entry selection.
	// -------------------------------------------------------------------------------------

	private void select(LTRect entry){
		LeanTween.move(entry, new Vector2(0f,0f), translationSpeed);
		LeanTween.scale(entry, topSize, scaleSpeed);

		// Move bars.
		LeanTween.move(LightBar, new Vector3(4f, -11.5f, -1.75f), 0.1f);
		LeanTween.move(DarkBar, new Vector3(1.85f, -10.1f, -1.1f), 0.2f);

		menuTranslate(selected);

		if(tempCount < 1){
					LeanTween.move(backMenu, new Vector2(backMenu.rect.x + width01, backMenu.rect.y), translationSpeed);
			click.GetComponent<AudioSource>().Play();
		}
	}

	private void menuTranslate(int selected){
		for(int i = 0; i < entries.Length; i++){
			if(i != selected){
				LeanTween.alpha(entries[i], 0f, alphaSpeed);
						LeanTween.move(entries[i], new Vector2(entries[i].rect.x - width + width01, entries[i].rect.y), translationSpeed);
			}
		}
	}

	private void reset(){
		// Hide the back bar.
		LeanTween.move(backMenu, new Vector2(backMenu.rect.x -width01, backMenu.rect.y), translationSpeed);

		// Restore all entries.
		for(int i = 0 ; i < entries.Length; i++){
			if(i != selected){
				LeanTween.alpha(entries[i], 1f, alphaSpeed);
				LeanTween.move(entries[i], new Vector2(leftMargin, entries[i].rect.y), translationSpeed);
			}
		}

		LeanTween.move(entries[selected], new Vector2(leftMargin, topMargin + selected * spacing), translationSpeed);
				LeanTween.scale(entries[selected], new Vector2(width03, height01), scaleSpeed);

		// Reset bars.
		resetBars();

		// Hide all menus.
		switch(selected){
		case 0:
			LeanTween.move(menus[0], new Vector2(width, menus[0].rect.y), translationSpeed);
			LeanTween.move(buttons[0], new Vector2(-(width * 0.6f), buttons[0].rect.y), translationSpeed);
			LeanTween.move(buttons[9], new Vector2(width, height02), translationSpeed);
			LeanTween.move(buttons[10], new Vector2(width, height02), translationSpeed);
        	break;

		case 1:
			LeanTween.move(menus[9], new Vector2(width, menus[9].rect.y), translationSpeed);
			break;

		case 2:
			LeanTween.move(buttons[6], new Vector2(width, buttons[6].rect.y), translationSpeed);
			LeanTween.move(menus[8], new Vector2(menus[8].rect.x, height), translationSpeed);
			break;

		case 3:
					LeanTween.move(buttons[7], new Vector2(buttons[7].rect.x,-height01), translationSpeed);
					LeanTween.move(buttons[8], new Vector2(buttons[8].rect.x, height + height01), translationSpeed);
			LeanTween.move(menus[6], new Vector2(width, menus[6].rect.y), translationSpeed);
			LeanTween.move(menus[7], new Vector2(menus[7].rect.x, height), translationSpeed);
			break;

		case 4:
			break;

		case 5:
			LeanTween.move(menus[1], new Vector2(width, menus[1].rect.y), translationSpeed);
			LeanTween.move(buttons[1], new Vector2(width, buttons[1].rect.y), translationSpeed);
			break;
		}

		tempCount = -1;
		selected = -1;
	}

	private void resetBars(){
		LeanTween.move(LightBar, new Vector3(-9.85f, 6.8f, -1.63f), 0.1f);
		LeanTween.move(DarkBar, new Vector3(-8.2f, 4.7f, -1.1f), 0.2f);
	}

	private void hangarSelection(){
		// Determine if ship is selected already.
		if(browseShip/2 == GameConfiguration.Instance.shipSelected){
			hangarSelected = "SELECTED";
		}
		else{
			hangarSelected = "SELECT";
		}
	}

	// ------------------------------------------------------------------
	// Load Highscores.
	// ------------------------------------------------------------------
	
	public void LoadHighscores(){
		HighscoreSaver.loadScores (this, HighscoreSaver.ScoreTypes.top10);
	}

	public void OnHighscoreLoaded(List<HighscoreSaver.Highscore> highscores)
	{
		this.highscores = highscores;
		Debug.Log ("Received");
	}
}