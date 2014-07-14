using UnityEngine;
using System.Collections;
using System.Collections;
using System.Collections.Generic;
using System;

/*
 * Author: Arnaud Durand
 * Do NOT modify this script without author acknowledgement
 */
public class AudioFXController : MonoBehaviour {
	
	//singleton
	private static AudioFXController instance;
	private static object padlock = new object();
	
	public static AudioFXController Instance
	{
		get {
			if (applicationIsQuitting)
				return null;

			if (instance == null)
			{
				lock(padlock){
					if (instance == null)
					{
						instance = new GameObject ("AudioFXController").AddComponent<AudioFXController> ();
					}	
				}
			}
			return instance;
		}
	}

	private static bool applicationIsQuitting = false;

	public void OnDestroy () {
		applicationIsQuitting = true;
	}
	/*
	public void OnApplicationQuit ()
	{
		instance = null;
	}
	 */
	//END singleton

	List<AudioFXHandler> listeners=new List<AudioFXHandler>();
	private float[] spectrum;

	void Update() {
		spectrum = AudioListener.GetSpectrumData(1024, 0, FFTWindow.BlackmanHarris);
		float intensity=float.NegativeInfinity;
		for (int i=0; i<7; i++){
			intensity=Mathf.Max(Mathf.Log(spectrum[i]),intensity);
		}

		NotifyAll(intensity);
	}

	public void NotifyAll(float value){
		foreach (AudioFXHandler listener in listeners){
			listener.UpdateFX(value);
		}
	}
	
	public static void RegisterListener(AudioFXHandler listener){
		Instance.listeners.Add(listener);
	}

	public static void UnregisterListener(AudioFXHandler listener){
		if (Instance!=null)
			Instance.listeners.Remove(listener);
	}





}
