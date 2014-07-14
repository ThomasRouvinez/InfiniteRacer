using UnityEngine;

public class ItemAnimator : MonoBehaviour {

	public bool upDown = false;

	void Start(){
		InvokeRepeating("Flip",0f,0.5f);
	}

	void Update (){
		transform.localRotation*=Quaternion.Euler(0f,Time.deltaTime*-120f,0f);
		transform.localPosition += (upDown ? Vector3.up : Vector3.down) * Time.deltaTime * 2.0f;
	}

	void Flip (){
		upDown=!upDown;
	}
}