using UnityEngine;
using System.Collections;

public class PipeFXHandler : AudioFXHandler {

	private float beatTimer=0f;

	public override void UpdateFX(float value){
		beatTimer = value > -4.7 ? 1f : beatTimer-Time.deltaTime;

		renderer.material.color = Color.Lerp(new Color(1f,0.5f,0f),new Color(0f,0.5f,1f),0f);
	}
}
