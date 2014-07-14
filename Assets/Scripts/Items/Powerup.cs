using UnityEngine;

public interface Powerup
{
	Texture2D icon { get; }
	void Trigger();
}