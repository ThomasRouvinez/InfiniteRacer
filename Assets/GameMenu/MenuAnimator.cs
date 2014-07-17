using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

public class MenuAnimator : MonoBehaviour {

	// -------------------------------------------------------------------------------------
	// Variables.
	// -------------------------------------------------------------------------------------

	private int width;
	private int height;
	private int leftMargin;
	private int topMargin;
	private int spacing;
	private int selected;
	private int creditSelected;
	private int tempCount;
	private int tempRandom;
	private int tmpFontSize;
	private bool tmpBool;
	private int browseShip;
	private string hangarSelected;

	public float translationSpeed;
	public float scaleSpeed;
	public float alphaSpeed;
	public float alphaSpeedCredits;

	public AudioSource click;
	public AudioSource highscoresRefreshButton;
	public AudioSource backClick;
	public Camera camera;
	public GameObject LightBar;
	public GameObject DarkBar;
	public GameObject earth;
	public AudioSource menuMusic;

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
	public GUISkin skinCreditsString;
	public GUISkin skinCreditsAttributions;
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
		entries[0] = new LTRect(leftMargin, topMargin,(width * 0.3f),(height * 0.1f));				// Race.
		entries[1] = new LTRect(leftMargin,(topMargin+1*spacing),(width * 0.3f),(height * 0.1f));	// Settings.
		entries[2] = new LTRect(leftMargin,(topMargin+2*spacing),(width * 0.3f),(height * 0.1f));	// Highscores.
		entries[3] = new LTRect(leftMargin,(topMargin+3*spacing),(width * 0.3f),(height * 0.1f));	// Hangar.
		entries[4] = new LTRect(leftMargin,(topMargin+4*spacing),(width * 0.3f),(height * 0.1f));	// Credits.
		entries[5] = new LTRect(leftMargin,(topMargin+5*spacing),(width * 0.3f),(height * 0.1f));	// Exit.

		menus = new LTRect[12];
		menus[0] = new LTRect(width, (height * 0.3f),(width * 0.85f), (height * 0.6f));					// Race container.
		menus[1] = new LTRect(width, (height * 0.3f),(width * 0.85f), (height * 0.6f));					// Exit container.
		menus[2] = new LTRect((width * 0.1f), (height * 0.2f), (width * 0.9f), (height * 0.8f));		// Arnaud's credits.
		menus[3] = new LTRect((width * 0.1f), (height * 0.2f), (width * 0.9f), (height * 0.8f));		// Thomas' credits.
		menus[4] = new LTRect((width * 0.1f), (height * 0.2f), (width * 0.9f), (height * 0.8f));		// Didier's credits.
		menus[5] = new LTRect((width * 0.1f), (height * 0.2f), (width * 0.9f), (height * 0.8f));		// Leonard's credits.
		menus[6] = new LTRect(width, (height * 0.1f), (width * 0.5f), (height * 0.8f));					// Ship's images.
		menus[7] = new LTRect((width * 0.1f), height, (width * 0.4f), (height * 0.8f));					// Ship's description.
		menus[8] = new LTRect((width * 0.17f), height, (width * 0.62f), (height * 0.7f));				// Highscores container.
		menus[9] = new LTRect(width, (height * 0.3f),(width * 0.85f), (height * 0.6f));					// Settings container.
		menus[10] = new LTRect((width * 0.1f), (height * 0.3f), (width * 0.9f), (height * 0.7f));		// Credits title + name container.
		menus[11] = new LTRect((width * 0.1f), (height * 0.4f), (width * 0.9f), (height * 0.6f));		// Credits attributions container.

		buttons = new LTRect[11];
		buttons[0] = new LTRect(-(width * 0.6f), (height * 0.9f),(width * 0.6f),(height * 0.1f));			// Start race button.
		buttons[1] = new LTRect(width, (height * 0.675f),(width * 0.6f),(height * 0.1f));					// Confirm exit button.
		buttons[2] = new LTRect((width * 0.5f),-(height * 0.2f),(width * 0.125f),(height * 0.2f));			// Arnaud credits button.
		buttons[3] = new LTRect((width * 0.625f),-(height * 0.2f),(width * 0.125f),(height * 0.2f));		// Thomas credits button.
		buttons[4] = new LTRect((width * 0.75f),-(height * 0.2f),(width * 0.125f),(height * 0.2f));			// Didier credits button.
		buttons[5] = new LTRect((width * 0.875f),-(height * 0.2f),(width * 0.125f),(height * 0.2f));		// Leonard credits button.
		buttons[6] = new LTRect(width + (width * 0.1f), (height * 0.25f), (width * 0.1f), (height * 0.7f));	// Highscore refresh button.
		buttons[7] = new LTRect((width * 0.5f), -(height * 0.15f), (width * 0.5f), (height * 0.1f));		// Ship selection next button.
		buttons[8] = new LTRect((width * 0.5f), height + (height * 0.15f), (width * 0.5f), (height * 0.1f));// Ship select button.
		buttons[9] = new LTRect(width, (height * 0.2f), (width * 0.42f), (height * 0.1f));					// Normal game mode.
		buttons[10] = new LTRect(width, (height * 0.2f), (width * 0.42f), (height * 0.1f));					// Hardcore game mode.

		backMenu = new LTRect(-(width * 0.1f), (height * 0.2f),(width * 0.1f),(height * 0.8f));
		backMenu.alpha = 0f;

		exitMessages = new string[]{"[Yes] I pretend I have work to do",
									"[Yes] I'm a sissie", 
									"[Yes] This is game is too hard for me"};

		creditsStrings = new string[]{"ARNAUD 'REXCORTEX' DURAND", 
									"THOMAS 'RASTE' ROUVINEZ", 
									"DIDIER 'CYBERNEMO' AEBERHARD", 
									"LEONARD 'LEOBITS' STALDER"};

		creditsAttributionStrings = new string[]{"\nPHYSICS PROGRAMMER\nGAMEPLAY PROGRAMMER\nPERFORMANCE OPTIMIZER\nSYSTEM ANALYST", 
									"\nCONCEPT ARTIST\n3D MODELLER\nTECHNICAL ARTIST\nGAMEPLAY PROGRAMMER\nLEAD USER INTERFACE", 
									"\nANIMATION PROGRAMMER\nGAMEPLAY PROGRAMMER\nAUDIO DESIGNER\nQA MANAGER", 
									"\nTOOLS PROGRAMMER\nDATABASE DESIGNER\nGAMEPLAY PROGRAMMER\nSERVER ARCHITECT"};
		
		top = new Vector2(0f, 0f);
		topSize = new Vector2((width * 0.5f), (height * 0.2f));
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
		if(GameConfiguration.Instance.menuMusicOn == false && menuMusic.audio.isPlaying){
			menuMusic.audio.Stop();
		}

		yield return new WaitForSeconds(0.2f);

		if(GameConfiguration.Instance.menuMusicOn == true && !menuMusic.audio.isPlaying){
			menuMusic.audio.Play();
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
			LeanTween.move(buttons[8], new Vector2(buttons[8].rect.x, height - (height * 0.1f)), translationSpeed).setEase(LeanTweenType.easeInOutBack);
			LeanTween.move(menus[6], new Vector2((width * 0.5f), menus[6].rect.y), translationSpeed * 1.5f);
			LeanTween.move(menus[7], new Vector2(menus[7].rect.x, (height * 0.2f)), translationSpeed * 2f);
		}

		if(GUI.Button(entries[4].rect, "<size=" + (entries[4].rect.width * 0.148) + ">CREDITS</size>")){
			selected = 4; tempCount++;
			select(entries[4]);

			LeanTween.move(buttons[2], new Vector2(buttons[2].rect.x, 0), 0.15f);
			LeanTween.move(buttons[3], new Vector2(buttons[3].rect.x, 0), 0.2f);
			LeanTween.move(buttons[4], new Vector2(buttons[4].rect.x, 0), 0.25f);
			LeanTween.move(buttons[5], new Vector2(buttons[5].rect.x, 0), 0.3f);

			LeanTween.move(menus[10], new Vector2((width * 0.1f), menus[10].rect.y), translationSpeed);

			for(int i = 2 ; i < 6 ; i++){
				if(i != creditSelected){
					LeanTween.alpha(menus[i], 0f, 0.1f);
				}
				else{
					LeanTween.alpha(menus[i], 1f, alphaSpeedCredits).setEase(LeanTweenType.easeInOutBack);
				}
			}
		}

		if(GUI.Button(entries[5].rect, "<size=" + (entries[5].rect.width * 0.15) + ">EXIT</size>")){
			selected = 5; tempCount++; tempRandom = Random.Range(0, exitMessages.Length);
			select(entries[5]);

			LeanTween.move(menus[1], new Vector2((width * 0.15f), menus[1].rect.y), translationSpeed).setEase(LeanTweenType.easeInOutBack);
			LeanTween.move(buttons[1], new Vector2((width * 0.275f), buttons[1].rect.y), translationSpeed).setEase(LeanTweenType.easeInOutBack);
		}

		GUI.skin = skinBackButton;

		if(GUI.Button(backMenu.rect, "")){
			backClick.audio.Play();
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

			GUI.Label(new Rect ((width * 0.2f), (height * 0.4f), (width * 0.7f), (height * 0.1f)), "<size=" + tmpFontSize + ">" + "ENABLE IN-GAME MUSIC</size>");
			GUI.Label(new Rect ((width * 0.2f), (height * 0.5f), (width * 0.7f), (height * 0.1f)), "<size=" + tmpFontSize + ">" + "ENABLE MENU MUSIC</size>");

			GameConfiguration.Instance.gameMusicOn = GUI.Toggle (new Rect ((width * 0.8f), (height * 0.4f), (height * 0.1f) ,(height * 0.1f)), GameConfiguration.Instance.gameMusicOn, "");
			GameConfiguration.Instance.menuMusicOn = GUI.Toggle (new Rect ((width * 0.8f), (height * 0.5f), (height * 0.1f) ,(height * 0.1f)), GameConfiguration.Instance.menuMusicOn, "");

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
							GUI.Box(new Rect((width * 0.6f),padd,(width * 0.2f), listHeight), "<size=" + tmpFontSize + ">" + hs.score + "</size>");
							padd += listHeight;
							count ++;
						}
						else{
							if(isIn == false){
								GUI.Box(new Rect((width * 0.18f),padd,(width * 0.05f), listHeight), "<size=" + tmpFontSize + ">>" + ">" + "</size>");
								GUI.Box(new Rect((width * 0.25f),padd,(width * 0.4f), listHeight), "<size=" + tmpFontSize + ">" + "YOUR BEST" + "</size>");
								GUI.Box(new Rect((width * 0.6f),padd,(width * 0.2f), listHeight), "<size=" + tmpFontSize + ">" + (long)PlayerPrefs.GetFloat("highscore", 0f) + "</size>");
								padd += listHeight;
								count ++;
								isIn = true;
							}

							if(count < 10){
								GUI.Box(new Rect((width * 0.18f),padd,(width * 0.05f), listHeight), "<size=" + tmpFontSize + ">" + hs.position + "</size>");
								GUI.Box(new Rect((width * 0.25f),padd,(width * 0.4f), listHeight), "<size=" + tmpFontSize + ">" + hs.name + "</size>");
								GUI.Box(new Rect((width * 0.6f),padd,(width * 0.2f), listHeight), "<size=" + tmpFontSize + ">" + hs.score + "</size>");
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
				GUI.Box(new Rect((width * 0.35f),(height * 0.45f),(width * 0.3f),(height * 0.1f)), "");	// Loading label.
			}

			GUI.skin = skinHighscoresButton;

			if(GUI.Button(buttons[6].rect, "")){	// Refresh button.
				highscoresRefreshButton.audio.Play();
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
			// Display 4 buttons.
			GUI.skin = creditHeads[0];
			if(GUI.Button(buttons[2].rect, "") && creditSelected != 2){		// Arnaud.
				LeanTween.alpha(menus[creditSelected], 0f, alphaSpeedCredits);
				LeanTween.alpha(menus[2], 1f, alphaSpeedCredits);
				creditSelected = 2;
			}

			GUI.skin = creditHeads[1];
			if(GUI.Button(buttons[3].rect, "") && creditSelected != 3){	// Thomas.
				LeanTween.alpha(menus[creditSelected], 0f, alphaSpeedCredits);
				LeanTween.alpha(menus[3], 1f, alphaSpeedCredits);
				creditSelected = 3;
			}

			GUI.skin = creditHeads[2];
			if(GUI.Button(buttons[4].rect, "") && creditSelected != 4){	// Didier.
				LeanTween.alpha(menus[creditSelected], 0f, alphaSpeedCredits);
				LeanTween.alpha(menus[4], 1f, alphaSpeedCredits);
				creditSelected = 4;
			}

			GUI.skin = creditHeads[3];
			if(GUI.Button(buttons[5].rect, "") && creditSelected != 5){	// Leonard.
				LeanTween.alpha(menus[creditSelected], 0f, alphaSpeedCredits);
				LeanTween.alpha(menus[5], 1f, alphaSpeedCredits);
				creditSelected = 5;
			}

			// Display the credit pannels.
			GUI.skin = skinCredits;
			tmpFontSize = (int) (menus[2].width * 0.05);

			GUI.Box(menus[2].rect, "<size=" + tmpFontSize + ">LEAD PROGRAMMER</size>");		// Credits AD.
			GUI.Box(menus[3].rect, "<size=" + tmpFontSize + ">LEAD ARTIST</size>");			// Credits TR.
			GUI.Box(menus[4].rect, "<size=" + tmpFontSize + ">SOFTWARE ENGINEER</size>");	// Credits DA.
			GUI.Box(menus[5].rect, "<size=" + tmpFontSize + ">SOFTWARE ENGINEER</size>");	// Credits LS.

			GUI.skin = skinCreditsString;
			GUI.Box(menus[10].rect, "<size=" + (menus[10].width * 0.03) + ">" + creditsStrings[creditSelected -2] + "</size>");
			GUI.skin = skinCreditsAttributions;
			GUI.Box(menus[11].rect, "<size=" + (menus[11].width * 0.03) + ">" + creditsAttributionStrings[creditSelected -2] + "</size>");

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
			LeanTween.move(backMenu, new Vector2(backMenu.rect.x + (width * 0.1f), backMenu.rect.y), translationSpeed);
			click.audio.Play();
		}
	}

	private void menuTranslate(int selected){
		for(int i = 0; i < entries.Length; i++){
			if(i != selected){
				LeanTween.alpha(entries[i], 0f, alphaSpeed);
				LeanTween.move(entries[i], new Vector2(entries[i].rect.x - width + (width * 0.1f), entries[i].rect.y), translationSpeed);
			}
		}
	}

	private void reset(){
		// Hide the back bar.
		LeanTween.move(backMenu, new Vector2(backMenu.rect.x -(width * 0.1f), backMenu.rect.y), translationSpeed);

		// Restore all entries.
		for(int i = 0 ; i < entries.Length; i++){
			if(i != selected){
				LeanTween.alpha(entries[i], 1f, alphaSpeed);
				LeanTween.move(entries[i], new Vector2(leftMargin, entries[i].rect.y), translationSpeed);
			}
		}

		LeanTween.move(entries[selected], new Vector2(leftMargin, topMargin + selected * spacing), translationSpeed);
		LeanTween.scale(entries[selected], new Vector2((width * 0.3f),(height * 0.1f)), scaleSpeed);

		// Reset bars.
		resetBars();

		// Hide all menus.
		switch(selected){
		case 0:
			LeanTween.move(menus[0], new Vector2(width, menus[0].rect.y), translationSpeed);
			LeanTween.move(buttons[0], new Vector2(-(width * 0.6f), buttons[0].rect.y), translationSpeed);
			LeanTween.move(buttons[9], new Vector2(width, (height * 0.2f)), translationSpeed);
			LeanTween.move(buttons[10], new Vector2(width, (height * 0.2f)), translationSpeed);
        	break;

		case 1:
			LeanTween.move(menus[9], new Vector2(width, menus[9].rect.y), translationSpeed);
			break;

		case 2:
			LeanTween.move(buttons[6], new Vector2(width, buttons[6].rect.y), translationSpeed);
			LeanTween.move(menus[8], new Vector2(menus[8].rect.x, height), translationSpeed);
			break;

		case 3:
			LeanTween.move(buttons[7], new Vector2(buttons[7].rect.x,-(height * 0.1f)), translationSpeed);
			LeanTween.move(buttons[8], new Vector2(buttons[8].rect.x, height + (height * 0.1f)), translationSpeed);
			LeanTween.move(menus[6], new Vector2(width, menus[6].rect.y), translationSpeed);
			LeanTween.move(menus[7], new Vector2(menus[7].rect.x, height), translationSpeed);
			break;

		case 4:
			LeanTween.move(buttons[2], new Vector2(buttons[2].rect.x, -(width * 0.2f)), translationSpeed);
			LeanTween.move(buttons[3], new Vector2(buttons[3].rect.x, -(width * 0.2f)), translationSpeed);
			LeanTween.move(buttons[4], new Vector2(buttons[4].rect.x, -(width * 0.2f)), translationSpeed);
			LeanTween.move(buttons[5], new Vector2(buttons[5].rect.x, -(width * 0.2f)), translationSpeed);
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