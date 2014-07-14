﻿/*
 * Author: Arnaud Durand
 * Do NOT modify this script without author acknowledgement
 */
public class GameConfiguration {

	// -------------------------------------------------------------------------------------
	// Variables.
	// -------------------------------------------------------------------------------------

	static GameConfiguration instance = null;
	static readonly object padlock = new object();

	GameConfiguration(){}

	// -------------------------------------------------------------------------------------
	// Initializes a single instance.
	// -------------------------------------------------------------------------------------

	public static GameConfiguration Instance{
		get{
			if (instance == null){
				lock (padlock){
					if (instance == null){
						instance = new GameConfiguration();
					}
				}
			}

			return instance;
		}
	}

	// -------------------------------------------------------------------------------------
	// Game init.
	// -------------------------------------------------------------------------------------

	public float speed;
	public float startSpeed = 120f;
	public float maxSpeed = 300f;

	public bool isShieldEnabled = false;
	public bool isOnPowerUp = false;
	public bool paused = false;
	public bool ended = false;

	public int coins = 0;
	public long score = 0;

	// Reward system.
	public long distance = 0;
	public int thresholdIndex = 0;
	public long[] thresholdValues = new long[]{5000,10000,20000,30000,50000};

	// Settings.
	public bool gameMusicOn;
	public bool menuMusicOn;
	public bool hardcoreMode = false;
	public short shipSelected = 0;
}