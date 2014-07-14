using UnityEngine;
using System.Collections;

/*
 * Author: Arnaud Durand
 */
public class ShieldHandler : MonoBehaviour, Powerup {

	public Texture2D _icon;

	public IEnumerator enableShield(float duration){
		GameConfiguration.Instance.isShieldEnabled = true;
		GameConfiguration.Instance.isOnPowerUp = true;
		Renderer shieldField = GameObject.Find("/Player/Shield Field").renderer;
		shieldField.enabled = true;
		float startTimer = Time.time;
		Color fieldColor=shieldField.material.color;

		while(startTimer+duration-1.4f > Time.time){
			fieldColor.a=Mathf.Sin(Time.time*5f)/2f+0.5f;
			shieldField.material.color=fieldColor;

			yield return null;
		}
		while(startTimer+duration-1.2f > Time.time){
			fieldColor.a=Mathf.Sin(Time.time*5f)/2f+0.5f;
			shieldField.material.color=fieldColor;
			shieldField.enabled = false;
			yield return null;
		}
		while(startTimer+duration-1.0f > Time.time){
			fieldColor.a=Mathf.Sin(Time.time*5f)/2f+0.5f;
			shieldField.material.color=fieldColor;	
			shieldField.enabled = true;
			yield return null;
		}
		while(startTimer+duration-0.8f > Time.time){
			fieldColor.a=Mathf.Sin(Time.time*5f)/2f+0.5f;
			shieldField.material.color=fieldColor;
			shieldField.enabled = false;
			yield return null;
		}
		while(startTimer+duration-0.6f > Time.time){
			fieldColor.a=Mathf.Sin(Time.time*5f)/2f+0.5f;
			shieldField.material.color=fieldColor;	
			shieldField.enabled = true;
			yield return null;
		}
		while(startTimer+duration-0.4f > Time.time){
			fieldColor.a=Mathf.Sin(Time.time*5f)/2f+0.5f;
			shieldField.material.color=fieldColor;	
			shieldField.enabled = false;
			yield return null;
		}
		while(startTimer+duration-0.2f > Time.time){
			fieldColor.a=Mathf.Sin(Time.time*5f)/2f+0.5f;
			shieldField.material.color=fieldColor;	
			shieldField.enabled = true;
			yield return null;
		}
		shieldField.enabled = false;
		Destroy(gameObject);
		GameConfiguration.Instance.isShieldEnabled = false;
		GameConfiguration.Instance.isOnPowerUp = false;
	}

	#region Powerup implementation
	public void Trigger ()
	{
		StartCoroutine(enableShield(7.5f));
	}

	public Texture2D icon {
		get {
			return _icon;
		}
	}
	#endregion
}
