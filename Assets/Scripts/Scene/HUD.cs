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

	public TextMesh energy;
	public TextMesh score;
	public TextMesh speed;

	private long tempScore = 0;
	private Gradient energyGrad;
	private GradientAlphaKey[] energyAlphaKey;
	private GradientColorKey[] energyColorKey;
	
	private Color energyHighColor;

	// ----------------------------------------------------------------------------
	// Get the required script references for the information displayed in the HUD.
	// ----------------------------------------------------------------------------

	void Start () {
		tempScore = 0;
		energyGrad = new Gradient();

		energyHighColor = new Color(69f, 225f, 40f, 1f);

		energyColorKey = new GradientColorKey[3];
		energyColorKey[0] = new GradientColorKey(Color.red, 0.20f);
		energyColorKey[1] = new GradientColorKey(Color.yellow, 0.6f);
		energyColorKey[2] = new GradientColorKey(Color.green, 1f);

		energyAlphaKey = new GradientAlphaKey[3];
		energyAlphaKey[0] = new GradientAlphaKey(1f,1f);
		energyAlphaKey[1] = new GradientAlphaKey(1f,1f);
		energyAlphaKey[2] = new GradientAlphaKey(1f,1f);

		energyGrad.SetKeys(energyColorKey, energyAlphaKey);
	}
	
	// Update fields in the HUD
	void Update () {
		// As long as we are not dead.
		if(!GameConfiguration.Instance.ended){
			// Update the fields.
			energy.text = GameConfiguration.Instance.energy.ToString("0.00");
			energy.color = energyGrad.Evaluate(GameConfiguration.Instance.energy / 100f);

			speed.text = ((short) (GameConfiguration.Instance.speed)).ToString();

			if(GameConfiguration.Instance.score > tempScore){
				tempScore += 1;
				score.text = tempScore.ToString();
			}
		}
	}
}