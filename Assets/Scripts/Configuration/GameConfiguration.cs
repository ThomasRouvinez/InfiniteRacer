/*
 * Author: Arnaud Durand
 * Do NOT modify this script without author acknowledgement
 * 
 * 	->>> really ? woops ;)
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
	public float energy = 100f;
	public float energyMax = 100f;
	public float startSpeed = 120f;
	public float maxSpeed = 300f;
	public float score = 0f;

	public bool isOnPowerUp = false;
	public bool paused = false;
	public bool started = false;
	public bool ended = false;
	public bool musicChanged = false;

	public byte causeOfDeath = 0;

	// Reward system.
	public long distance = 0;
	public int thresholdIndex = 0;
	public long[] thresholdValues = new long[]{2000,5000,10000,20000,30000,40000,50000,75000,100000,150000,200000, 250000, 500000};

	// Settings.
	public bool gameMusicOn;
	public bool menuMusicOn;
	public bool hardcoreMode = false;
	public int shipSelected = 0;
}