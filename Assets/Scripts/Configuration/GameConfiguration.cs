/*
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
	public float energy = 100f;
	public float energyMax = 100f;
	public float startSpeed = 120f;
	public float maxSpeed = 300f;

	public bool isShieldEnabled = false;
	public bool isOnPowerUp = false;
	public bool paused = false;
	public bool ended = false;

	public int coins = 0;
	public float score = 0f;

	// Reward system.
	public long distance = 0;
	public int thresholdIndex = 0;
	public long[] thresholdValues = new long[]{5000,10000,25000,50000,100000};

	// Settings.
	public bool gameMusicOn;
	public bool menuMusicOn;
	public bool hardcoreMode = false;
	public int shipSelected = 0;
}