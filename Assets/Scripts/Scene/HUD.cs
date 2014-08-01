using UnityEngine;
using System.Collections;

/*
 * Author : Thomas Rouvinez
 * Description : class to handle HUD information and displayal.
 * 
 */
public class HUD : MonoBehaviour {

	// ----------------------------------------------------------------------------
	// Variables.
	// ----------------------------------------------------------------------------

	public TextMesh coinNumber;
	public TextMesh score;
	public TextMesh speed;
	private long tempScore = 0;

	// ----------------------------------------------------------------------------
	// Get the required script references for the information displayed in the HUD.
	// ----------------------------------------------------------------------------

	void Start () {
		tempScore = 0;
	}
	
	// Update fields in the HUD
	void Update () {
		// As long as we are not dead.
		if(!GameConfiguration.Instance.ended){
			// Update the fields.
			coinNumber.text = GameConfiguration.Instance.energy.ToString("0.00") ;

			speed.text = ((short) (GameConfiguration.Instance.speed)).ToString() ;

			if(GameConfiguration.Instance.score > tempScore){
				tempScore += 1;
				score.text = tempScore.ToString(); 
			}
		}
	}
}