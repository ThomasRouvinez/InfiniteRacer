using UnityEngine;
using System.Collections;

public abstract class AudioFXHandler : MonoBehaviour {

	public abstract void UpdateFX(float value);

	// Use this for initialization
	IEnumerator Start () {
		yield return AudioFXController.Instance;

		AudioFXController.RegisterListener(this);
	}

	void OnDestroy () {
		AudioFXController.UnregisterListener(this);
	}
}
