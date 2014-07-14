using UnityEngine;
using System.Collections;

public class ObstacleFXHandler : AudioFXHandler {

	private float beatTimer=0f;
	
	public override void UpdateFX(float value){
		beatTimer = Mathf.Clamp01(value > -3.5 ? 1f : beatTimer-Time.deltaTime*3);
		renderer.material.color = Color.Lerp(new Color(1f,0.5f,0f),new Color(0f,0.5f,1f),beatTimer);
	}
}
